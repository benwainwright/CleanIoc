namespace CleanIoc.Metadata.Entities.Base
{
    using CleanIoc.Metadata.Enums;

    public class LocalTypeEntity : TypeEntity
    {
        public LocalTypeEntity(string name, string theNamespace, int token)
            : base(name, token)
        {
            Namespace = theNamespace;
        }

        public LocalTypeEntity(string name, string theNamespace)
            : base(name)
        {
            Namespace = theNamespace;
        }

        public LocalTypeEntity(int token)
            : base(token)
        {
        }

        public string Namespace { get; }
    }
}
