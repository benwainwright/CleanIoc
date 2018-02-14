namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ICleanIocContainer : IDisposable
    {
        T Get<T>() where T : class;

        IList<T> GetAll<T>() where T : class;

        IServiceProvider ServiceProvider { get; }
    }
}
