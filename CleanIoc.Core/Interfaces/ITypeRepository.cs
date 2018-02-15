namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Implementation;

    public interface ITypeRepository : IDisposable
    {
        void AddRegistryContents(ITypeRegistry registry);

        void AddRegistration(ITypeRegistration registration);

        IList<object> GetInstances(Type from, out List<IInjectedType> failed);
    }
}
