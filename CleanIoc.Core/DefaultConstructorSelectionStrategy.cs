using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CleanIoc.Core.Interfaces;

namespace CleanIoc.Core
{
    internal class DefaultConstructorSelectionStrategy : IConstructorSelectionStrategy
    {
        public ConstructorInfo SelectConstructor(Type type, IList<ConstructorInfo> remainingConstructors)
        {
            // TODO for now lets just return the first one in the list

            if(remainingConstructors.Count > 0) {
                return remainingConstructors[0];
            }

            return null;
        }
    }
}
