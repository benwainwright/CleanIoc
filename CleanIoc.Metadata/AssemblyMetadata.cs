namespace CleanIoc.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Runtime.InteropServices;
    using CleanIoc.Metadata.Entities;
    using CleanIoc.Metadata.Entities.Base;

    public class AssemblyMetadata : IDisposable
    {
        private readonly MetadataReader reader;

        private bool disposed = false;

        private byte[] buffer;

        private GCHandle pinnedHandle;

        private List<AssemblyRef> assemblyReferences;

        private List<TypeDef> typeDefinitions;

        private List<TypeRef> typeReferences;

        public AssemblyMetadata(string path)
        {
            FilePath = path;
            reader = LoadMetadataReader(FilePath);
            ParseAll();
        }

        public string FilePath { get; }

        public string Name { get; private set; }

        public Version Version { get; private set; }

        public List<byte> PublicKey { get; private set; }

        public IEnumerable<AssemblyRef> AssemblyReferences => assemblyReferences;

        public IEnumerable<TypeDef> TypeDefinitions => typeDefinitions;

        public IEnumerable<TypeRef> TypeReferences => typeReferences;

        public void LinkAssemblies(List<AssemblyMetadata> assemblies)
        {
            foreach (var assembly in AssemblyReferences) {
                assembly.LinkAssemblyMetaData(assemblies);
            }

            foreach (var reference in TypeReferences) {
                reference.ResolveTypesFromLinkedAssembly();
            }
        }

        public void ParseAll()
        {
            ParseAssemblyDefinition();
            assemblyReferences = AssemblyRef.LoadReferences(reader);
            typeDefinitions = TypeDef.LoadDefinitions(reader);
            typeReferences = TypeRef.LoadReferences(reader, assemblyReferences);
            LinkBaseTypes(typeDefinitions, typeReferences);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return $"{Name} {Version}";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed) {
                if (disposing && reader != null) {
                    pinnedHandle.Free();
                }

                disposed = true;
            }
        }

        private static void LinkBaseTypes(IEnumerable<TypeDef> definitions, IEnumerable<TypeRef> references)
        {
            var allTypes = new List<LocalTypeEntity>();
            allTypes.AddRange(definitions);
            allTypes.AddRange(references);

            foreach (var type in definitions) {
                type.LinkBaseType(allTypes);
                type.LinkInterfaceImplementations(allTypes);
            }
        }

        private void ParseAssemblyDefinition()
        {
            var assembly = reader.GetAssemblyDefinition();
            Name = reader.GetString(assembly.Name);
            Version = assembly.Version;
            PublicKey = reader.GetBlobBytes(assembly.PublicKey).ToList();
        }

        private unsafe MetadataReader LoadMetadataReader(
            string filename,
            MetadataReaderOptions options = MetadataReaderOptions.Default,
            MetadataStringDecoder decoder = null)
        {
            buffer = File.ReadAllBytes(filename);
            pinnedHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var headers = new PEHeaders(new MemoryStream(buffer));
            var startOffset = headers.MetadataStartOffset;
            var metaDataStart = (byte*)pinnedHandle.AddrOfPinnedObject() + startOffset;
            return new MetadataReader(metaDataStart, headers.MetadataSize, options, decoder);
        }
    }
}
