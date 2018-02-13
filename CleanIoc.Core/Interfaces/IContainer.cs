using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanIoc.Core.Interfaces
{
    public interface IContainer
    {
        T Get<T>() where T : class;

        IList<T> GetAll<T>() where T : class;
    }
}
