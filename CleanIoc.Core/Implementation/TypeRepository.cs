namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Exceptions;
    using CleanIoc.Core.Interfaces;

    internal class TypeRepository : ITypeRepository
    {
        private readonly Dictionary<Type, HashSet<TypeConstructionPlan>> allPlans = new Dictionary<Type, HashSet<TypeConstructionPlan>>();

        private readonly IConstructorSelectionStrategy constructorSelectionStrategy;

        private bool disposed = false;

        public TypeRepository(IConstructorSelectionStrategy constructorSelectionStrategy = null)
        {
            this.constructorSelectionStrategy = constructorSelectionStrategy ?? new DefaultConstructorSelectionStrategy();
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
            if (registration == null) {
                throw new ArgumentNullException($"{nameof(registration)} cannot be null");
            }

            if (!allPlans.ContainsKey(registration.DefinedType)) {
                allPlans[registration.DefinedType] = new HashSet<TypeConstructionPlan>();
            }

            allPlans[registration.DefinedType].Add(new TypeConstructionPlan(registration, allPlans, constructorSelectionStrategy));
        }

        public IList<object> GetInstances(Type from, out List<IInjectedType> failed)
        {
            var returnVal = new List<object>();
            failed = new List<IInjectedType>();
            HashSet<TypeConstructionPlan> plans;
            try {
                plans = allPlans[from];
            } catch (KeyNotFoundException ex) {
                throw new MappingNotFoundException($"No mapping was found for type {from.FullName}", ex);
            }

            foreach (var plan in plans) {
                if (plan.CanBeConstructed()) {
                    returnVal.Add(plan.GetInstance());
                } else {
                    failed.Add(plan);
                }
            }

            return returnVal;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // TODO - dispose of disposable singletons
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed) {
                if (disposing) {
                }

                disposed = true;
            }
        }
    }
}