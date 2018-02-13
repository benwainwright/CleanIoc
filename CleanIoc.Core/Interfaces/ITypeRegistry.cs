using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanIoc.Core.Enums;

namespace CleanIoc.Core.Interfaces
{
    public interface ITypeRegistry
    {
        IEnumerable<KeyValuePair<Type, List<Type>>> RegisteredTypes { get; }

        void MapBetween<TFrom, TTo>(Lifetime lifetime = Lifetime.Singleton);

        void ConstructorStrategy(IConstructorSelectionStrategy strategy);
    }
}
