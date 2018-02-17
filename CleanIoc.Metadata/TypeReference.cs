using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace CleanIoc.Metadata
{

    public class TypeReference : TypeEntity
    {
        public TypeReference(MetadataReader reader, TypeReferenceHandle handle, IEnumerable<AssemblyReference> assemblies)
            : base(
                reader.GetString(reader.GetTypeReference(handle).Name),
                reader.GetString(reader.GetTypeReference(handle).Namespace),
                reader.GetToken(handle))
        {
            var reference = reader.GetTypeReference(handle);
            Assembly = GetAssembly(assemblies, reader.GetToken(reference.ResolutionScope));
        }

        public AssemblyReference Assembly { get; }

        public static List<TypeReference> LoadReferences(MetadataReader reader, IEnumerable<AssemblyReference> assemblies)
        {
            var query = from type
                        in reader.TypeReferences
                        select new TypeReference(reader, type, assemblies);

            return query.ToList();
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        private static AssemblyReference GetAssembly(IEnumerable<AssemblyReference> assemblies, int token)
        {
            var query = from assembly
                        in assemblies
                        where assembly.Token == token
                        select assembly;

            return query.FirstOrDefault();
        }
    }
}
