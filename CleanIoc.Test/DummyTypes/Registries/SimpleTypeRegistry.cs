using CleanIoc.Core.Test.DummyTypes.Concrete;
using CleanIoc.Core.Test.DummyTypes.Interfaces;

namespace CleanIoc.Core.Test.DummyTypes.Registries
{
    public class SimpleTypeRegistry : TypeRegistry
    {
        public SimpleTypeRegistry()
        {
            Register<ISimpleInterface>().With<EmptyClassWithDefaultConstructor>().AsSingleton();
            Register<ISecondInterface>().With<EmptyClassWithThatOneSimpleObjectInItsConstructor>() .AsSingleton();
            Register<IThirdInterface>().With<MoreComplicatedClassThatCantBeFullySatisfied>().AsSingleton();
        }
    }
}
