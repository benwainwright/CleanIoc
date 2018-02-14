namespace CleanIoc.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

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
