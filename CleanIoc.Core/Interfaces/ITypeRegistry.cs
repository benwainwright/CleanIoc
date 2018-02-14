namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;    
    using CleanIoc.Core.Enums;

    public interface ITypeRegistry
    {
        IEnumerable<KeyValuePair<Type, List<Type>>> RegisteredTypes { get; }

        IMappable Map<TFrom>(Lifetime lifetime = Lifetime.Singleton);

        void ConstructorStrategy(IConstructorSelectionStrategy strategy);
    }
}
