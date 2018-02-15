namespace CleanIoc.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CleanIoc.Core.Interfaces;

    public class UnableToConstructException : CleanIocException
    {
        public UnableToConstructException()
        {
        }

        public UnableToConstructException(IList<IConstructorAttempt> constructors)
        {
            AttemptedConstructors = constructors;
        }

        public UnableToConstructException(IList<IConstructorAttempt> constructors, string message)
            : base(message)
        {
            AttemptedConstructors = constructors;
        }

        public UnableToConstructException(IList<IConstructorAttempt> constructors, string message, Exception innerException)
            : base(message, innerException)
        {
            AttemptedConstructors = constructors;
        }

        protected UnableToConstructException(IList<IConstructorAttempt> constructors, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            AttemptedConstructors = constructors;
        }

        public IList<IConstructorAttempt> AttemptedConstructors { get; set; }
    }
}
