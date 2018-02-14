using CleanIoc.Core.Test.DummyTypes.Concrete;
using CleanIoc.Core.Test.DummyTypes.Interfaces;

namespace CleanIoc.Core.Test.DummyTypes.Registries
{
    public class SimpleTypeRegistry : TypeRegistry
    {
        public SimpleTypeRegistry()
        {
            Map<ISimpleInterface>().To<EmptyClassWithDefaultConstructor>();
            Map<ISecondInterface>().To<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            Map<IThirdInterface>().To<MoreComplicatedClassThatCantBeFullySatisfied>();
        }
    }
}
