namespace CleanIoc.Core
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Implementation;
    using CleanIoc.Core.Interfaces;

    public abstract class TypeRegistry : ITypeRegistry, IMappable
    {
        private readonly Dictionary<Type, List<ITypeRegistration>> types = new Dictionary<Type, List<ITypeRegistration>>();

        private Type lastFromType;

        public IEnumerable<KeyValuePair<Type, List<ITypeRegistration>>> Registrations => types;

        private IConstructorSelectionStrategy Strategy { get; set; }

        public void ConstructorStrategy(IConstructorSelectionStrategy strategy)
        {
            Strategy = strategy;
        }

        public IMappable Register<TFrom>(InstanceLifetime lifetime = InstanceLifetime.Singleton)
        {
            lastFromType = typeof(TFrom);
            return this;
        }

        public ITypeRegistration WithConcreteType<TTo>()
        {
            if (!types.ContainsKey(lastFromType)) {
                types[lastFromType] = new List<ITypeRegistration>();
            }

            var registration = new TypeRegistration(lastFromType, typeof(TTo));
            types[lastFromType].Add(registration);
            return registration;
        }
    }
}
