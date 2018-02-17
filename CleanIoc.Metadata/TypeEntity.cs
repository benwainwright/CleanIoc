namespace CleanIoc.Metadata
{
    public abstract class TypeEntity : EntityWithToken
    {
        public TypeEntity(string name, string theNamespace, int token)
            : base(token)
        {
            Name = name;
            Namespace = theNamespace;
        }

        public string Name { get; }

        public string Namespace { get; }
    }
}
