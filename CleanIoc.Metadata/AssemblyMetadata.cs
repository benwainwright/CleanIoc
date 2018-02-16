namespace CleanIoc.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Runtime.InteropServices;

    public class AssemblyMetadata : IDisposable
    {
        private MetadataReader reader;

        private bool disposed = false;

        private byte[] buffer;

        private GCHandle pinnedHandle;

        private List<AssemblyReference> assemblyReferences;

        public AssemblyMetadata(string path)
        {
            FilePath = path;
        }

        public IEnumerable<AssemblyReference> AssemblyReferences
        {
            get
            {
                if (assemblyReferences == null) {
                    assemblyReferences = AssemblyReference.LoadReferences(Reader);
                }

                return assemblyReferences;
            }
        }

        public string FilePath { get; }

        private MetadataReader Reader { get
            {
                if (reader == null) {
                    reader = LoadMetadataReader(FilePath);
                }

                return reader;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
