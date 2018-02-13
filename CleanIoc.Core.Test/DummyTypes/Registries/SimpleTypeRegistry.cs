using CleanIoc.Core.Test.DummyTypes.Concrete;
using CleanIoc.Core.Test.DummyTypes.Interfaces;

namespace CleanIoc.Core.Test.DummyTypes.Registries
{
    public class SimpleTypeRegistry : TypeRegistry
    {
        public SimpleTypeRegistry()
        {
            MapBetween<ISimpleInterface, EmptyClassWithDefaultConstructor>();
            MapBetween<ISecondInterface, EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            MapBetween<IThirdInterface, MoreComplicatedClassThatCantBeFullySatisfied>();
        }
    }
}
