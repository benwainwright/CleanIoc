using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanIoc.Core.Interfaces
{
    public interface IMappable
    {
        void To<TTo>();
    }
}
