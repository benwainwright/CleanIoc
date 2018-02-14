using CleanIoc.Core.Implementation;
using CleanIoc.Core.Test.DummyTypes.Concrete;
using CleanIoc.Core.Test.DummyTypes.Interfaces;
using CleanIoc.Core.Test.DummyTypes.Registries;
using NUnit.Framework;

namespace CleanIoc.Core.Test
{
    [TestFixture]
    public class TestContainer
    {
        [Test]
        public void TestThatItWorksWithASimpleTypeWithADefaultConstructor()
        {
            var registry = new SimpleTypeRegistry();
            registry.Map<ISimpleInterface>().To<EmptyClassWithDefaultConstructor>();
            var builder = new ContainerBuilder(Enums.ScanBehaviour.Off);
            builder.AddRegistry(registry);
            var container = builder.Container;

            var instance = container.Get<ISimpleInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.TypeOf<EmptyClassWithDefaultConstructor>());
        }

        [Test]
        public void TestThatWithoutPassingInALifeTimeItReturnsASingleton()
        {
            var registry = new SimpleTypeRegistry();
            registry.Map<ISimpleInterface>().To<EmptyClassWithDefaultConstructor>();
            var builder = new ContainerBuilder(Enums.ScanBehaviour.Off);
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            var second = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);
            Assert.That(first, Is.SameAs(second));
        }

        [Test]
        public void TestThatItCanConstructTypesWithASingleRegistredType()
        {
            var registry = new SimpleTypeRegistry();
            registry.Map<ISimpleInterface>().To<EmptyClassWithDefaultConstructor>();
            registry.Map<ISecondInterface>().To<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            var builder = new ContainerBuilder(Enums.ScanBehaviour.Off);
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);
            Assert.That(first, Is.TypeOf<EmptyClassWithDefaultConstructor>());

            var second = container.Get<ISecondInterface>();
            Assert.That(second, Is.Not.Null);
            Assert.That(second, Is.TypeOf<EmptyClassWithThatOneSimpleObjectInItsConstructor>());
            Assert.That(second.FirstParam, Is.Not.Null);
            Assert.That(second.FirstParam, Is.SameAs(first));
        }

        [Test]
        public void TestThrowsAnExceptionIfCantFullySatisfyConstructor()
        {
            var registry = new SimpleTypeRegistry();
            registry.Map<ISimpleInterface>().To<EmptyClassWithDefaultConstructor>();
            registry.Map<ISecondInterface>().To<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            registry.Map<IThirdInterface>().To<MoreComplicatedClassThatCantBeFullySatisfied>();
            var builder = new ContainerBuilder(Enums.ScanBehaviour.Off);
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);

            var second = container.Get<ISecondInterface>();
            Assert.That(second, Is.Not.Null);
        }

        [Test]
        public void TestRegistryIsFoundAutomatically()
        {
            var builder = new ContainerBuilder();
            builder.AddAssemblyLoader(new RegistryAssemblyLoader());
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);

            var second = container.Get<ISecondInterface>();
            Assert.That(second, Is.Not.Null);
        }
    }
}
