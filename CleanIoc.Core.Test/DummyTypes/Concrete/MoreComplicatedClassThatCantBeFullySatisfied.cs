using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanIoc.Core.Test.DummyTypes.Interfaces;

namespace CleanIoc.Core.Test.DummyTypes.Concrete
{
    public class MoreComplicatedClassThatCantBeFullySatisfied : IThirdInterface
    {
        public MoreComplicatedClassThatCantBeFullySatisfied(ISecondInterface paramOne, IFourthInterfaceActuallyDoesntHaveAnyDerivedClasses two)
        {
        }
    }
}
