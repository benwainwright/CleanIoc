namespace CleanIoc.Metadata.Entities
{
    using CleanIoc.Metadata.Enums;

    public class TypeEntity : EntityWithToken
    {
        public TypeEntity(string name, string theNamespace, int token)
            : base(token, ResolutionStatus.Resolved)
        {
            Name = name;
            Namespace = theNamespace;
        }

        public TypeEntity(string name, string theNamespace)
        {
            Name = name;
            Namespace = theNamespace;
        }

        public TypeEntity(int token)
            : base(token, ResolutionStatus.UnResolved)
        {
        }

        public string Name { get; }

        public string Namespace { get; }
    }
}
