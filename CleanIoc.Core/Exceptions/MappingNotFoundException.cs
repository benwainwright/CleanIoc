namespace CleanIoc.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class MappingNotFoundException : CleanIocException
    {
        public MappingNotFoundException()
        {
        }

        public MappingNotFoundException(string message) : base(message)
        {
        }

        public MappingNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MappingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
