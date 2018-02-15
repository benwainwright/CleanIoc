namespace CleanIoc.Core.Implementation.Construction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Utils;

    internal class ConstructorAttempt : IConstructorAttempt, IExecutableConstructor
    {
        private Type type;

        private IList<ConstructionPlan> plans;

        public ConstructorAttempt(Type type, List<ConstructionPlan> parameters, bool success)
        {
            Guard.Against.Null(type, nameof(type));
            Guard.Against.Null(parameters, nameof(parameters));

            plans = parameters;
            Success = success;
            this.type = type;
        }

        public IEnumerable<IInjectedType> Parameters => plans;

        public bool Success { get; }

        public object Execute()
        {
            var values = from plan
                         in plans
                         select plan.GetInstance();

            return Activator.CreateInstance(type, values.ToArray());
        }
    }
}
