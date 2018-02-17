namespace CleanIoc.Metadata.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;

    public class TypeRef : TypeEntity
    {
        public TypeRef(MetadataReader reader, TypeReferenceHandle handle, IEnumerable<AssemblyRef> assemblies)
            : base(
                reader.GetString(reader.GetTypeReference(handle).Name),
                reader.GetString(reader.GetTypeReference(handle).Namespace),
                reader.GetToken(handle))
        {
            var reference = reader.GetTypeReference(handle);
            Assembly = GetAssembly(assemblies, reader.GetToken(reference.ResolutionScope));


        }

        public AssemblyRef Assembly { get; }

        public TypeDef ResolvedType { get; private set; }

        public static List<TypeRef> LoadReferences(MetadataReader reader, IEnumerable<AssemblyRef> assemblies)
        {
            var query = from type
                        in reader.TypeReferences
                        select new TypeRef(reader, type, assemblies);

            return query.ToList();
        }

        public void ResolveTypesFromLinkedAssembly()
        {
            if (ResolvedType != null || Assembly == null) {
                return;
            }

            var metaData = Assembly?.MetaData;
            if (metaData == null) {
                return;
            }

            var defined = metaData.TypeDefinitions;

            var query = from type
                        in defined
                        where type.Name.Equals(Name, StringComparison.InvariantCulture) &&
                              type.Namespace.Equals(Namespace, StringComparison.InvariantCulture)
                        select type;

            ResolvedType = query.FirstOrDefault();
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        private static AssemblyRef GetAssembly(IEnumerable<AssemblyRef> assemblies, int token)
        {
            var query = from assembly
                        in assemblies
                        where assembly.Token == token
                        select assembly;

            return query.FirstOrDefault();
        }
    }
}
