namespace CleanIoc.Core.Test.DummyTypes.Registries
{
    using CleanIoc.Core.Test.DummyTypes.Concrete;
    using CleanIoc.Core.Test.DummyTypes.Interfaces;

    public class SimpleTypeRegistry : Registry
    {
        public SimpleTypeRegistry()
        {
            Register<ISimpleInterface>().WithConcreteType<EmptyClassWithDefaultConstructor>().AsSingleton();
            Register<ISecondInterface>().WithConcreteType<EmptyClassWithThatOneSimpleObjectInItsConstructor>().AsSingleton();
            Register<IThirdInterface>().WithConcreteType<MoreComplicatedClassThatCantBeFullySatisfied>().AsSingleton();
        }
    }
}
