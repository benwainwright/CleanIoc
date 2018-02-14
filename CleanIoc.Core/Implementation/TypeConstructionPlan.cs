namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CleanIoc.Core.Enums;

    internal class TypeConstructionPlan : IInjectedParameter
    {
        private readonly Dictionary<Type, TypeMap> maps;

        public Type Declared { get; }

        public Type Injected { get; private set; }

        private MultipleMappingsBehaviour MultipleMappingsBehaviour { get; }

        private TypeConstructionPlan root;

        public List<IConstructorAttempt> ConstructorAttempts { get; } = new List<IConstructorAttempt>();

        private IExecutableConstructor constructorToUse;

        public ConstructionOutcome Outcome { get; private set; }

        public TypeConstructionPlan(Type from, Dictionary<Type, TypeMap> maps, MultipleMappingsBehaviour multipleMappingsBehaviour = MultipleMappingsBehaviour.FailConstruction)
        {
            this.maps = maps;
            this.root = root ?? this;
            Declared = from;
            MultipleMappingsBehaviour = multipleMappingsBehaviour;
        }

        public bool CanBeConstructed(Type with = null)
        {
            if (Outcome == ConstructionOutcome.Success) {
                return true;
            }
            if(!maps.ContainsKey(Declared)) {
                Outcome = ConstructionOutcome.NoMappingFound;
                return false;
            }
            if (maps[Declared].Size > 1 && MultipleMappingsBehaviour == MultipleMappingsBehaviour.FailConstruction) {
                Outcome = ConstructionOutcome.MultipleMappings;
                return false;
            }
            Injected = with != null ? with : maps[Declared].Types.FirstOrDefault();
            var constructors = Injected.GetConstructors().ToList();
            ConstructorAttempt attempt;
            do {
                var constructor = maps[Declared].ConstructorSelector.SelectConstructor(Declared, constructors);
                attempt = PlanConstructorExecution(constructor);
                ConstructorAttempts.Add(attempt);
            } while (constructors.Count > 0 && !attempt.Success);

            if (attempt.Success) {
                constructorToUse = attempt;
                Outcome = ConstructionOutcome.Success;
                return true;
            } else {
                Outcome = ConstructionOutcome.MultipleMappings;
                return false;
            }
        }

        private ConstructorAttempt PlanConstructorExecution(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            bool success = true;
            var constructionPlans = new List<TypeConstructionPlan>();
            foreach(var parameter in parameters) {
                var plan = new TypeConstructionPlan(parameter.ParameterType, maps);
                success = !plan.CanBeConstructed() ? false : success;
                constructionPlans.Add(plan);
            }
            return new ConstructorAttempt(Injected, constructionPlans, success);
        }

        public object GetInstance()
        {
            return constructorToUse.execute();
        }
    }
}
