namespace CleanIoc.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class CleanIocException : Exception
    {
        public CleanIocException()
        {
        }

        public CleanIocException(string message) : base(message)
        {
        }

        public CleanIocException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CleanIocException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
