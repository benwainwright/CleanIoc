namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;

    public interface IRegistry
    {
        IEnumerable<KeyValuePair<Type, List<IRegistration>>> Registrations { get; }

        IMappable Register<TFrom>(InstanceLifetime lifetime = InstanceLifetime.Singleton);

        void ConstructorStrategy(IConstructorSelectionStrategy strategy);
    }
}
