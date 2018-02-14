namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Exceptions;
    using CleanIoc.Core.Interfaces;

    internal class TypeRepository : ITypeRepository
    {
        private Dictionary<Type, TypeMap> Maps = new Dictionary<Type, TypeMap>();

        private IConstructorSelectionStrategy ConstructorSelectionStrategy { get; set; }

        public TypeRepository(IConstructorSelectionStrategy constructorSelectionStrategy = null)
        {
            ConstructorSelectionStrategy = constructorSelectionStrategy != null
                ? constructorSelectionStrategy
                : new DefaultConstructorSelectionStrategy();
        }

        public void AddRegistryContents(ITypeRegistry registry)
        {
            foreach (var entry in registry.RegisteredTypes) {
                foreach (var toType in entry.Value) {
                    AddTypeMapping(entry.Key, toType);
                }
            }
        }

        public void AddTypeMapping(Type from, Type to, Lifetime lifetime = Lifetime.Singleton)
        {
            if (!Maps.ContainsKey(from)) {
                Maps[from] = new TypeMap(from, lifetime);
            }
            Maps[from].Add(to);
        }

        public IList<object> GetInstances(Type from)
        {
            var returnVal = new List<object>();
            TypeMap typemap;
            try {
                typemap = Maps[from];
            } catch(KeyNotFoundException ex) {
                throw new TypeSupplyException(string.Format("No mapping was found for type {0}", from.FullName), ex);
            }
            if (typemap.Size > 0) {
                var types = new List<Type>(typemap.Types);
                foreach (var type in types) {
                    object instance = null;
                    if(typemap.Lifetime == Lifetime.Singleton) {
                        instance = typemap.GetLoadedInstanceOf(type);
                        if(instance == null) {
                            instance = MakeInstance(type);
                            typemap.AddLoadedInstanceOf(type, instance);
                        }
                    } else {
                        instance = MakeInstance(type);
                    }
                    if (instance != null) {
                        returnVal.Add(instance);
                    }
                }
            }
            return returnVal;
        }

        private object MakeInstance(Type type)
        {
            // Always prefer the default constructor; this will effectively
            // be our base case for the recursion that is going on here
            object instance;
            try {
                return Activator.CreateInstance(type);
            } catch (MissingMethodException) {
                // Carry on
            }
            var constructors = new List<ConstructorInfo>(type.GetConstructors());
            do {
                var constructor = ConstructorSelectionStrategy.SelectConstructor(type, constructors);
                instance = TryToMakeInstanceWithConstructor(type, constructor);
                constructors.Remove(constructor);
            } while (instance == null && constructors.Count > 0);

            if (instance == null) {
                throw new ArgumentException($"Could not create instance of {type.Name} as was unable to find a matching constructor");
            }

            return instance;
        }

        private object TryToMakeInstanceWithConstructor(Type type, ConstructorInfo constructor)
        {
            if (type == null || constructor == null) {
                return null;
            }

            var parameters = constructor.GetParameters();
            var values = new List<object>();

            foreach (var param in parameters) {
                IList<object> instances = GetInstances(param.ParameterType);
                if(instances.Count == 0) {
                    return null;
                }
                values.Add(instances[0]);
            }

            return Activator.CreateInstance(type, values.ToArray());
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}