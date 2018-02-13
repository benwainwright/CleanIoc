namespace CleanIoc.Core
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;

    public abstract class TypeRegistry : ITypeRegistry
    {
        public IEnumerable<KeyValuePair<Type, List<Type>>> RegisteredTypes { get { return types;  } }

        private Dictionary<Type, List<Type>> types = new Dictionary<Type, List<Type>>();

        private IConstructorSelectionStrategy Strategy { get; set; }

        public void ConstructorStrategy(IConstructorSelectionStrategy strategy)
        {
            Strategy = strategy;
        }

        public void MapBetween<TFrom, TTo>(Lifetime lifetime = Lifetime.Singleton)
        {
            if(!types.ContainsKey(typeof(TFrom))) {
                types[typeof(TFrom)] = new List<Type>();
            }
            types[typeof(TFrom)].Add(typeof(TTo));
        }
    }
}
