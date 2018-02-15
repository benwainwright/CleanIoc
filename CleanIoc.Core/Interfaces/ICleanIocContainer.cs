namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ICleanIocContainer : IDisposable
    {
        IServiceProvider ServiceProvider { get; }

        T GetInstanceOf<T>()
            where T : class;

        IList<T> GetAll<T>()
            where T : class;
    }
}
