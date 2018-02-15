using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanIoc.Core.Implementation
{
    public interface IConstructorAttempt
    {
        IEnumerable<IInjectedType> Parameters { get; }

        bool Success { get; }
    }
}
