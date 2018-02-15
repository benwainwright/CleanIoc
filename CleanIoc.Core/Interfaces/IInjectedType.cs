using System;
using System.Collections.Generic;
using CleanIoc.Core.Enums;

namespace CleanIoc.Core.Implementation
{
    public interface IInjectedType
    {
        Type Declared { get; }

        Type Injected { get; }

        List<IConstructorAttempt> ConstructorAttempts { get; }

        ConstructionOutcome Outcome { get; }
    }
}
