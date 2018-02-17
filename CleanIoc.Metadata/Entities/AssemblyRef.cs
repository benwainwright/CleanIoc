/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
namespace MetadataScanner.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;
    using MetadataScanner.Base;

    public class AssemblyRef : TypeEntity
    {
        public AssemblyRef(MetadataReader reader, AssemblyReferenceHandle handle)
            : base(
                  reader.GetString(reader.GetAssemblyReference(handle).Name),
                  reader.GetToken(handle))
        {
            var reference = reader.GetAssemblyReference(handle);
            Culture = reader.GetString(reference.Culture);
            PublicKeyOrToken = reader.GetBlobBytes(reference.PublicKeyOrToken).ToList();
            Version = reference.Version;
            Flags = reference.Flags;
        }

        public AssemblyMetadata MetaData { get; private set; }

        public string Culture { get; }

        public List<byte> PublicKeyOrToken { get; }

        public Version Version { get; }

        public AssemblyFlags Flags { get; }

        public static List<AssemblyRef> LoadReferences(MetadataReader reader)
        {
            var query = from reference
                        in reader.AssemblyReferences
                        select new AssemblyRef(reader, reference);

            return query.ToList();
        }

        public void LinkAssemblyMetaData(IEnumerable<AssemblyMetadata> assemblies)
        {
            if (MetaData != null) {
                return;
            }

            var query = from assembly
                        in assemblies
                        where assembly.Name.Equals(Name, StringComparison.InvariantCulture) &&
                              assembly.PublicKey.SequenceEqual(PublicKeyOrToken) &&
                              assembly.Version.Equals(Version)
                        select assembly;

            MetaData = query.FirstOrDefault();
        }

        public override string ToString()
        {
            return $"{Name} {Version}";
        }
    }
}
