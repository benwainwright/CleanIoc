namespace CleanIoc.Metadata
{

    public abstract class EntityWithToken
    {
        public EntityWithToken(int token)
        {
            Token = token;
        }

        public int Token { get; }
    }
}
