using System;
using System.Collections.Generic;

namespace CleanIoc.Core.Interfaces
{
    public interface ICleanIocContainer
    {
        T Get<T>() where T : class;

        IList<T> GetAll<T>() where T : class;

        IServiceProvider ServiceProvider { get; };
    }
}
