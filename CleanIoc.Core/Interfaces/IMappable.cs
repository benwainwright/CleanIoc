using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanIoc.Core.Implementation;

namespace CleanIoc.Core.Interfaces
{
    public interface IMappable
    {
        ITypeRegistration With<TTo>();
    }
}
