namespace CleanIoc.Metadata
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;

    public class TypeDefinition : TypeEntity
    {
        private readonly System.Reflection.Metadata.TypeDefinition definition;

        private readonly int baseTypeToken;

        public TypeDefinition(MetadataReader reader, TypeDefinitionHandle handle)
            : base(
                reader.GetString(reader.GetTypeDefinition(handle).Name),
                reader.GetString(reader.GetTypeDefinition(handle).Namespace),
                reader.GetToken(handle))
        {
            definition = reader.GetTypeDefinition(handle);
            baseTypeToken = reader.GetToken(definition.BaseType);
        }

        public TypeEntity BaseType { get; private set; }

        public static List<TypeDefinition> LoadDefinitions(MetadataReader reader)
        {
            var query = from definition
                        in reader.TypeDefinitions
                        select new TypeDefinition(reader, definition);

            return query.ToList();
        }

        public void LinkBaseType(List<TypeEntity> types)
        {
            var query = from type
                        in types
                        where type.Token == baseTypeToken
                        select type;

            if (query.Any()) {
                BaseType = query.First();
            }
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
