namespace CleanIoc.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;

    public abstract class TypeRegistry : ITypeRegistry, IMappable
    {
        private List<Expression> expressions = new List<Expression>();

        public IEnumerable<KeyValuePair<Type, List<Type>>> RegisteredTypes { get { return types;  } }

        private Type fromType;

        private Dictionary<Type, List<Type>> types = new Dictionary<Type, List<Type>>();

        private IConstructorSelectionStrategy Strategy { get; set; }

        public void ConstructorStrategy(IConstructorSelectionStrategy strategy)
        {
            Strategy = strategy;
        }

        public IMappable Map<TFrom>(Lifetime lifetime = Lifetime.Singleton)
        {
            fromType = typeof(TFrom);
            return this;
        }

        public void To<TTo>()
        {
            if (!types.ContainsKey(fromType)) {
                types[fromType] = new List<Type>();
            }
            types[fromType].Add(typeof(TTo));
        }
    }
}
