namespace CleanIoc.Metadata.Entities
{
    using CleanIoc.Metadata.Enums;

    public abstract class EntityWithToken
    {
        public EntityWithToken(int token, ResolutionStatus resolutionStatus)
        {
            Token = token;
            ResolutionStatus = resolutionStatus;
        }

        public EntityWithToken(ResolutionStatus resolutionStatus)
        {
            ResolutionStatus = resolutionStatus;
        }

        public int Token { get; }

        public ResolutionStatus ResolutionStatus { get; }
    }
}
