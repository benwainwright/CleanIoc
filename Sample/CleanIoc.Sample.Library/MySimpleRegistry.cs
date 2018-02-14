namespace CleanIoc.Sample.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CleanIoc.Core;
    using CleanIoc.Sample.Interfaces;

    public class MySimpleRegistry : TypeRegistry
    {
        public MySimpleRegistry()
        {
            MapBetween<IMySimpleSampleInterface, MySimpleClass>();
        }
    }
}
