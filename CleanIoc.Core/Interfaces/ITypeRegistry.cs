namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;    
    using CleanIoc.Core.Enums;

    public interface ITypeRegistry
    {
        IEnumerable<KeyValuePair<Type, List<ITypeRegistration>>> Registrations { get; }

        IMappable Register<TFrom>(Lifetime lifetime = Lifetime.Singleton);

        void ConstructorStrategy(IConstructorSelectionStrategy strategy);
    }
}
