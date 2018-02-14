namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;

    public interface ITypeRepository : IDisposable
    {
        void AddRegistryContents(ITypeRegistry registry);

        void AddTypeMapping(Type from, Type to, Lifetime lifetime = Lifetime.Singleton);

        IList<object> GetInstances(Type from);
    }
}
