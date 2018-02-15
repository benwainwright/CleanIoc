namespace CleanIoc.Sample.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CleanIoc.Core;
    using CleanIoc.Sample.Interfaces;

    public class MySimpleRegistry : Registry
    {
        public MySimpleRegistry()
        {
            Register<IMySimpleSampleInterface>().WithConcreteType<MySimpleClass>();
        }
    }
}
