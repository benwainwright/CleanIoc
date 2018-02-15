namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;

    internal class TypeConstructionPlan : IInjectedType
    {
        public Type Declared { get; }

        private readonly Dictionary<Type, HashSet<TypeConstructionPlan>> plans;

        public Type Injected { get; private set; }

        private List<Type> possibleMappings = new List<Type>();

        public IEnumerable<Type> PossibleMappings => possibleMappings;

        private MultipleMappingsBehaviour MultipleMappingsBehaviour { get; }

        public List<IConstructorAttempt> ConstructorAttempts { get; } = new List<IConstructorAttempt>();

        private IExecutableConstructor constructorToUse;

        public ConstructionOutcome Outcome { get; private set; }

        private readonly IConstructorSelectionStrategy constructorSelector;

        private readonly Lifetime lifetime;

        private object instance;

        public TypeConstructionPlan(ITypeRegistration registration, Dictionary<Type, HashSet<TypeConstructionPlan>> otherPlans, IConstructorSelectionStrategy selector, MultipleMappingsBehaviour multipleMappingsBehaviour = MultipleMappingsBehaviour.FailConstruction)
        {
            if(registration == null) {
                throw new ArgumentNullException($"{nameof(registration)} cannot be null");
            }
            Declared = registration.From;
            Injected = registration.To;
            MultipleMappingsBehaviour = multipleMappingsBehaviour;
            plans = otherPlans;
            constructorSelector = selector;
            lifetime = registration.Lifetime;
        }

        public bool CanBeConstructed()
        {
            if (Outcome == ConstructionOutcome.Success) {
                return true;
            }

            if(!plans.ContainsKey(Declared)) {
                Outcome = ConstructionOutcome.NoMappingFound;
                return false;
            }

            if (plans[Declared].Count > 1 && MultipleMappingsBehaviour == MultipleMappingsBehaviour.FailConstruction) {
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

        private ConstructorAttempt TryConstructorsOf(Type type)
        {
            var constructors = Injected.GetConstructors().ToList();
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
            var constructionPlans = new List<TypeConstructionPlan>();
            foreach(var parameter in parameters) {
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

        public override bool Equals(object other)
        {
            return Equals(other as TypeConstructionPlan);
        }

        public bool Equals(TypeConstructionPlan other)
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
                return Declared.GetHashCode() +
                       Injected.GetHashCode() * 17;
            }
        }

        public object GetInstance()
        {
            if (lifetime == Lifetime.Singleton) {
                if (instance == null) {
                    instance = constructorToUse.execute();
                }
                return instance;
            }
            return constructorToUse.execute();
        }
    }
}
