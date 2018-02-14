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
        private Dictionary<Type, TypeMap> maps = new Dictionary<Type, TypeMap>();

        private Dictionary<Type, TypeConstructionPlan> plans = new Dictionary<Type, TypeConstructionPlan>();

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
            if (!maps.ContainsKey(from)) {
                maps[from] = new TypeMap(from, lifetime, ConstructorSelectionStrategy);
            }
            maps[from].Add(to);
            if (plans.Count > 0) {
                plans.Clear();
            }
        }

        public IList<object> GetInstances(Type from)
        {
            var returnVal = new List<object>();
            TypeMap typemap;
            try {
                typemap = maps[from];
            } catch(KeyNotFoundException ex) {
                throw new MappingNotFoundException(string.Format("No mapping was found for type {0}", from.FullName), ex);
            }
            if (typemap.Size > 0) {
                var types = new List<Type>(typemap.Types);
                foreach (var type in types) {
                    if(!plans.ContainsKey(type)) {
                        plans[type] = new TypeConstructionPlan(from, maps);
                    }
                    if(plans[type].CanBeConstructed(with: type)) {
                        returnVal.Add(plans[type].GetInstance());
                    }
                }
            }
            return returnVal;
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