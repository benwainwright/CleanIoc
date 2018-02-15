﻿namespace CleanIoc.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CleanIoc.Core.Implementation;

    public class UnableToConstructException : CleanIocException
    {
        public IList<IConstructorAttempt> AttemptedConstructors { get; set; }

        public UnableToConstructException()
        {
        }

        public UnableToConstructException(IList<IConstructorAttempt> constructors)
        {
        }

        public UnableToConstructException(IList<IConstructorAttempt> constructors, string message) : base(message)
        {
            AttemptedConstructors = constructors;
        }

        public UnableToConstructException(IList<IConstructorAttempt> constructors, string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToConstructException(IList<IConstructorAttempt> constructors, SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
