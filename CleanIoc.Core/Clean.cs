namespace CleanIoc.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CleanIoc.Core.Implementation;

    public static class Clean
    {
        public static IContainerBuilder MakeBuilder()
        {
            return new ContainerBuilder();
        }
    }
}
