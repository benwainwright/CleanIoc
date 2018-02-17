namespace CleanIoc.Metadata.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;
    using CleanIoc.Metadata.Enums;
    using CleanIoc.Metadata.Entities.Base;

    public class TypeDef : LocalTypeEntity
    {
        private readonly System.Reflection.Metadata.TypeDefinition definition;

        public TypeDef(MetadataReader reader, TypeDefinitionHandle handle)
            : base(
                reader.GetString(reader.GetTypeDefinition(handle).Name),
                reader.GetString(reader.GetTypeDefinition(handle).Namespace),
                reader.GetToken(handle))
        {
            definition = reader.GetTypeDefinition(handle);
            BaseType = new TypeEntity(reader.GetToken(definition.BaseType));
            foreach (var implementation in definition.GetInterfaceImplementations()) {
                var newInterface = new TypeEntity(reader.GetToken(reader.GetInterfaceImplementation(implementation).Interface));
                InterfaceImplementation.Add(newInterface);
            }
        }

        public TypeEntity BaseType { get; private set; }

        public List<TypeEntity> InterfaceImplementation { get; private set; } = new List<TypeEntity>();

        public static List<TypeDef> LoadDefinitions(MetadataReader reader)
        {
            var query = from definition
                        in reader.TypeDefinitions
                        select new TypeDef(reader, definition);

            return query.ToList();
        }

        public void LinkBaseType(List<TypeEntity> types)
        {
            if (BaseType.ResolutionStatus == ResolutionStatus.Resolved) {
                return;
            }

            var query = from type
                        in types
                        where type.Token == BaseType.Token
                        select type;

            BaseType = query.FirstOrDefault();
        }

        public void LinkInterfaceImplementations(List<TypeEntity> types)
        {

        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
