using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanIoc.Core.Implementation;
using CleanIoc.Sample.Interfaces;

namespace CleanIoc.Sample.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            var container = builder.Container;
            var instance = container.Get<IMySimpleSampleInterface>();

            Console.WriteLine(string.Format("The message is '{0}'", instance.Message));

        }
    }
}
