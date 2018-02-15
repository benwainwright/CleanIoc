namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using CleanIoc.Core.Utils;

    internal class ConstructorAttempt : IConstructorAttempt, IExecutableConstructor
    {
        public IEnumerable<IInjectedType> Parameters => plans;

        private IList<TypeConstructionPlan> plans;

        public bool Success { get; }

        private Type type;

        public ConstructorAttempt(Type type, List<TypeConstructionPlan> parameters, bool success)
        {
            Guard.Against.Null(type, nameof(type));
            Guard.Against.Null(parameters, nameof(parameters));

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
