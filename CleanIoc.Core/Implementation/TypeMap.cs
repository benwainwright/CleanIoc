namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;

    internal class TypeMap
    {
        private Type InterfaceType { get; }

        public Lifetime Lifetime { get; }

        private IDictionary<Type, object> InitialisedTypes { get; } = new Dictionary<Type, object>();

        public int Size { get { return InitialisedTypes.Count; } }

        public IEnumerable<Type> Types { get { return InitialisedTypes.Keys; } }

        public TypeMap(Type interfaceType, Lifetime lifetime)
        {
            InterfaceType = interfaceType;
            Lifetime = lifetime;
        }

        public void Add(Type type)
        {
            if(type.Assembly.ReflectionOnly) {
                throw new ArgumentException("Return type must be from fully loaded assembly");
            }

            if(!InterfaceType.IsAssignableFrom(type)) {
                throw new ArgumentException("Return type must be assignable from the provided interface type");
            }
            InitialisedTypes[type] = null;
        }

        public void AddLoadedInstanceOf(Type type, object instance)
        {
            if(!InitialisedTypes.ContainsKey(type)) {
                throw new ArgumentException("Mapping does not contain concrete types");
            }
            InitialisedTypes[type] = instance;
        }

        public object GetLoadedInstanceOf(Type type)
        {
            if (!InitialisedTypes.ContainsKey(type)) {
                throw new ArgumentException("Mapping does not contain concrete types");
            }
            return InitialisedTypes[type];
        }
    }
}
