namespace CleanIoc.Metadata.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;
    using CleanIoc.Metadata.Entities.Base;
    using CleanIoc.Metadata.Enums;

    public class TypeRef : LocalTypeEntity
    {
        public TypeRef(MetadataReader reader, TypeReferenceHandle handle, IEnumerable<AssemblyRef> assemblies)
            : base(
                reader.GetString(reader.GetTypeReference(handle).Name),
                reader.GetString(reader.GetTypeReference(handle).Namespace),
                reader.GetToken(handle))
        {
            var reference = reader.GetTypeReference(handle);
            Assembly = GetAssembly(assemblies, reader.GetToken(reference.ResolutionScope));
            Definition = new TypeDef(Name, Namespace);
        }

        public AssemblyRef Assembly { get; }

        public TypeDef Definition { get; private set; }

        public static List<TypeRef> LoadReferences(MetadataReader reader, IEnumerable<AssemblyRef> assemblies)
        {
            var query = from type
                        in reader.TypeReferences
                        select new TypeRef(reader, type, assemblies);

            return query.ToList();
        }

        public void ResolveTypesFromLinkedAssembly()
        {
            if (Definition.ResolutionStatus == ResolutionStatus.Resolved || Assembly == null) {
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

            Definition = query.FirstOrDefault();
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
