﻿namespace CleanIoc.Core
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Implementation;
    using CleanIoc.Core.Interfaces;

    public abstract class TypeRegistry : ITypeRegistry, IMappable
    {
        public IEnumerable<KeyValuePair<Type, List<ITypeRegistration>>> Registrations { get { return types;  } }

        private Dictionary<Type, List<ITypeRegistration>> types = new Dictionary<Type, List<ITypeRegistration>> ();

        private Type lastFromType;

        private IConstructorSelectionStrategy Strategy { get; set; }

        public void ConstructorStrategy(IConstructorSelectionStrategy strategy)
        {
            Strategy = strategy;
        }

        public IMappable Register<TFrom>(Lifetime lifetime = Lifetime.Singleton)
        {
            lastFromType = typeof(TFrom);
            return this;
        }

        public ITypeRegistration With<TTo>()
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
