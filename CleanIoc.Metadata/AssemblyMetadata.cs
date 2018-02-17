/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
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

    public class AssemblyMetadata
    {
        private readonly MetadataReader reader;

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
            pinnedHandle.Free();
        }

        public override string ToString()
        {
            return $"{Name} {Version}";
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
