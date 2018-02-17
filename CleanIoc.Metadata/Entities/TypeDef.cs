namespace CleanIoc.Metadata.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;
    using CleanIoc.Metadata.Entities.Base;
    using CleanIoc.Metadata.Enums;

    public class TypeDef : LocalTypeEntity
    {
        private readonly TypeDefinition definition;

        public TypeDef(MetadataReader reader, TypeDefinitionHandle handle)
            : base(
                reader.GetString(reader.GetTypeDefinition(handle).Name),
                reader.GetString(reader.GetTypeDefinition(handle).Namespace),
                reader.GetToken(handle))
        {
            definition = reader.GetTypeDefinition(handle);
            BaseType = new TypeEntity(reader.GetToken(definition.BaseType));
            foreach (var implementation in definition.GetInterfaceImplementations()) {
                var newInterface = new LocalTypeEntity(reader.GetToken(reader.GetInterfaceImplementation(implementation).Interface));
                InterfaceImplementations.Add(newInterface);
            }
        }

        public TypeDef(string name, string theNamespace)
            : base(name, theNamespace)
        {
        }

        public TypeEntity BaseType { get; private set; }

        public List<LocalTypeEntity> InterfaceImplementations { get; private set; } = new List<LocalTypeEntity>();

        public static List<TypeDef> LoadDefinitions(MetadataReader reader)
        {
            var query = from definition
                        in reader.TypeDefinitions
                        select new TypeDef(reader, definition);

            return query.ToList();
        }

        public void LinkBaseType(List<LocalTypeEntity> types)
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

        public void LinkInterfaceImplementations(List<LocalTypeEntity> types)
        {
            var unresolved = from implementation
                             in InterfaceImplementations
                             where implementation.ResolutionStatus == ResolutionStatus.UnResolved
                             select implementation;

            var remove = new List<LocalTypeEntity>();
            var resolved = new List<LocalTypeEntity>();

            foreach (var implementation in unresolved.ToList()) {
                var query = from type
                            in types
                            where type.Token == implementation.Token
                            select type;

                if (query.Any()) {
                    remove.Add(implementation);
                    resolved.Add(query.First());
                }
            }

            foreach (var unresolvedItem in remove) {
                InterfaceImplementations.Remove(unresolvedItem);
            }

            InterfaceImplementations.AddRange(resolved);
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
