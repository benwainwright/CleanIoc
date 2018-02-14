namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using CleanIoc.Core.Interfaces;

    internal class DefaultConstructorSelectionStrategy : IConstructorSelectionStrategy
    {
        public ConstructorInfo SelectConstructor(Type type, IList<ConstructorInfo> remainingConstructors)
        {
            int max = -1;
            ConstructorInfo returnVal = null;
            foreach(var constructor in remainingConstructors) {
                var paramaters = constructor.GetParameters();
                if (paramaters.Length > max) {
                    returnVal = constructor;
                    max = paramaters.Length;
                }
            }
            return returnVal;
        }
    }
}
