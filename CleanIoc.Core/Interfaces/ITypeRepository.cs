using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanIoc.Core.Enums;

namespace CleanIoc.Core.Interfaces
{
    public interface ITypeRepository
    {
        void AddRegistryContents(ITypeRegistry registry);

        void AddTypeMapping(Type from, Type to, Lifetime lifetime = Lifetime.Singleton);

        IList<object> GetInstances(Type from);
    }
}
