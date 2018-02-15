namespace CleanIoc.Core.Test.DummyTypes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CleanIoc.Core.Test.DummyTypes.Interfaces;

    public class EmptyClassWithThatOneSimpleObjectInItsConstructor : ISecondInterface
    {
        public EmptyClassWithThatOneSimpleObjectInItsConstructor(ISimpleInterface param)
        {
            FirstParam = param;
        }

        public ISimpleInterface FirstParam { get; set; }
    }
}
