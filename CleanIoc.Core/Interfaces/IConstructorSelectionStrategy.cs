namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IConstructorSelectionStrategy
    {
        ConstructorInfo SelectConstructor(Type type, IList<ConstructorInfo> remainingConstructors);
    }
}
