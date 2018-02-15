namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Exceptions;
    using CleanIoc.Core.Interfaces;

    internal class TypeRepository : ITypeRepository
    {
        private Dictionary<Type, TypeMap> maps = new Dictionary<Type, TypeMap>();

        private Dictionary<Type, HashSet<TypeConstructionPlan>> allPlans = new Dictionary<Type, HashSet<TypeConstructionPlan>>();

        private IConstructorSelectionStrategy ConstructorSelectionStrategy { get; set; }

        public TypeRepository(IConstructorSelectionStrategy constructorSelectionStrategy = null)
        {
            ConstructorSelectionStrategy = constructorSelectionStrategy != null
                ? constructorSelectionStrategy
                : new DefaultConstructorSelectionStrategy();
        }

        public void AddRegistryContents(ITypeRegistry registry)
        {
            foreach (var entry in registry.Registrations) {
                foreach (var toType in entry.Value) {
                    AddRegistration(toType);
                }
            }
        }

        public void AddRegistration(ITypeRegistration registration)
        {
            if(registration == null) {
                throw new ArgumentNullException($"{nameof(registration)} cannot be null");
            }
            if (!allPlans.ContainsKey(registration.From)) {
                allPlans[registration.From] = new HashSet<TypeConstructionPlan>();
            }

            allPlans[registration.From].Add(new TypeConstructionPlan(registration, allPlans, ConstructorSelectionStrategy));
        }

        public IList<object> GetInstances(Type from, out List<IInjectedType> failed)
        {
            var returnVal = new List<object>();
            failed = new List<IInjectedType>();
            HashSet<TypeConstructionPlan> plans;
            try {
                plans = allPlans[from];
            } catch(KeyNotFoundException ex) {
                throw new MappingNotFoundException(string.Format("No mapping was found for type {0}", from.FullName), ex);
            }
            foreach(var plan in plans) {
                if(plan.CanBeConstructed()) {
                    returnVal.Add(plan.GetInstance());
                } else {
                    failed.Add(plan);
                }
            }
            return returnVal;
        }

        private bool disposedValue = false;

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