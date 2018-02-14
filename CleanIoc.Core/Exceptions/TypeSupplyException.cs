namespace CleanIoc.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;


    public class TypeSupplyException : CleanIocException
    {
        public TypeSupplyException()
        {
        }

        public TypeSupplyException(string message) : base(message)
        {
        }

        public TypeSupplyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeSupplyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
