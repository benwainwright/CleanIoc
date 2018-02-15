namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Implementation;

    public interface IRepository : IDisposable
    {
        void AddRegistryContents(IRegistry registry);

        void AddRegistration(IRegistration registration);

        IList<object> GetInstances(Type from, out List<IInjectedType> failed);
    }
}
