namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class ConstructorAttempt : IConstructorAttempt, IExecutableConstructor
    {
        public IEnumerable<IInjectedType> Parameters => plans;

        private IList<TypeConstructionPlan> plans;

        public bool Success { get; }

        private Type type;

        public ConstructorAttempt(Type type, List<TypeConstructionPlan> parameters, bool success)
        {
            plans = parameters;
            Success = success;
            this.type = type;
        }

        public object execute()
        {
            var values = from plan
                         in plans
                         select plan.GetInstance();

            return Activator.CreateInstance(type, values.ToArray());
        }
    }
}
