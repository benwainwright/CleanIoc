namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Utils;

    internal class ConstructionPlan : IInjectedType
    {
        private readonly Dictionary<Type, HashSet<ConstructionPlan>> plans;

        private readonly IConstructorSelectionStrategy constructorSelector;

        private readonly MultipleMappingsBehaviour multipleMappingsBehaviour;

        private readonly InstanceLifetime lifetime;

        private List<Type> possibleMappings = new List<Type>();

        private IExecutableConstructor constructorToUse;

        private object instance;

        public ConstructionPlan(IRegistration registration, Dictionary<Type, HashSet<ConstructionPlan>> otherPlans, IConstructorSelectionStrategy selector, MultipleMappingsBehaviour multipleMappingsBehaviour = MultipleMappingsBehaviour.FailConstruction)
        {
            Guard.Against.Null(registration, nameof(registration));
            Guard.Against.Null(otherPlans, nameof(otherPlans));
            Guard.Against.Null(selector, nameof(selector));

            Declared = registration.DefinedType;
            Injected = registration.InjectedType;
            this.multipleMappingsBehaviour = multipleMappingsBehaviour;
            plans = otherPlans;
            constructorSelector = selector;
            lifetime = registration.Lifetime;
        }

        public Type Declared { get; }

        public Type Injected { get; private set; }

        public IEnumerable<Type> PossibleMappings => possibleMappings;

        public List<IConstructorAttempt> ConstructorAttempts { get; } = new List<IConstructorAttempt>();

        public ConstructionOutcome Outcome { get; private set; }

        public bool CanBeConstructed()
        {
            if (Outcome == ConstructionOutcome.Success) {
                return true;
            }

            if (!plans.ContainsKey(Declared)) {
                Outcome = ConstructionOutcome.NoMappingFound;
                return false;
            }

            if (plans[Declared].Count > 1 && multipleMappingsBehaviour == MultipleMappingsBehaviour.FailConstruction) {
                Outcome = ConstructionOutcome.MultipleMappings;
                return false;
            }

            var finalConstructorAttempt = TryConstructorsOf(Injected);

            if (finalConstructorAttempt.Success) {
                constructorToUse = finalConstructorAttempt;
                Outcome = ConstructionOutcome.Success;
                return true;
            } else {
                Outcome = ConstructionOutcome.NoSuitableConstructor;
                return false;
            }
        }

        public object GetInstance()
        {
            if (lifetime == InstanceLifetime.Singleton) {
                if (instance == null) {
                    instance = constructorToUse.Execute();
                }

                return instance;
            }

            return constructorToUse.Execute();
        }

        public override bool Equals(object other)
        {
            return Equals(other as ConstructionPlan);
        }

        public bool Equals(ConstructionPlan other)
        {
            if (other == null) {
                return false;
            }

            if (other == this) {
                return true;
            }

            return other.Declared == Declared &&
                   other.Injected == Injected;
        }

        public override int GetHashCode()
        {
            unchecked {
                return Declared.GetHashCode() + (Injected.GetHashCode() * 17);
            }
        }

        private ConstructorAttempt TryConstructorsOf(Type type)
        {
            var constructors = type.GetConstructors().ToList();
            ConstructorAttempt attempt;
            do {
                var constructor = constructorSelector.SelectConstructor(Declared, constructors);
                attempt = PlanConstructorExecution(constructor);
                ConstructorAttempts.Add(attempt);
                constructors.Remove(constructor);
            } while (constructors.Count > 0 && !attempt.Success);

            return attempt;
        }

        private ConstructorAttempt PlanConstructorExecution(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            bool success = true;
            var constructionPlans = new List<ConstructionPlan>();
            foreach (var parameter in parameters) {
                var type = parameter.ParameterType;
                if (plans.ContainsKey(type)) {
                    var plan = plans[type].FirstOrDefault();
                    success = !plan.CanBeConstructed() ? false : success;
                    constructionPlans.Add(plan);
                } else {
                    success = false;
                }
            }

            return new ConstructorAttempt(Injected, constructionPlans, success);
        }
    }
}
