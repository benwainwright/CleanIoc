namespace CleanIoc.Metadata.Entities.Base
{
    using CleanIoc.Metadata.Enums;

    public class TypeEntity : EntityWithToken
    {
        public TypeEntity(string name, int token)
            : base(token, ResolutionStatus.Resolved)
        {
            Name = name;
        }

        public TypeEntity(string name)
            : base(ResolutionStatus.UnResolved)
        {
            Name = name;
        }

        public TypeEntity(int token)
            : base(token, ResolutionStatus.UnResolved)
        {
        }

        public string Name { get; }
    }
}
