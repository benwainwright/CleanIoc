﻿using System.Linq;
using CleanIoc.Core.Enums;
using CleanIoc.Core.Exceptions;
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
            registry.Register<ISimpleInterface>().With<EmptyClassWithDefaultConstructor>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var instance = container.Get<ISimpleInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.TypeOf<EmptyClassWithDefaultConstructor>());
        }

        [Test]
        public void TestThatWithoutSettingALifetimeItIsTransient()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().With<EmptyClassWithDefaultConstructor>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            var second = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);
            Assert.That(first, Is.Not.SameAs(second));
        }

        [Test]
        public void TestThatItCanConstructTypesWithASingleRegistredType()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().With<EmptyClassWithDefaultConstructor>();
            registry.Register<ISecondInterface>().With<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);
            Assert.That(first, Is.TypeOf<EmptyClassWithDefaultConstructor>());

            var second = container.Get<ISecondInterface>();
            Assert.That(second, Is.Not.Null);
            Assert.That(second, Is.TypeOf<EmptyClassWithThatOneSimpleObjectInItsConstructor>());
            Assert.That(second.FirstParam, Is.Not.Null);
            Assert.That(second.FirstParam, Is.InstanceOf<EmptyClassWithDefaultConstructor>());
        }

        [Test]
        public void TestThrowsAnExceptionIfCantFullySatisfyConstructor()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().With<EmptyClassWithDefaultConstructor>();
            registry.Register<ISecondInterface>().With<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            registry.Register<IThirdInterface>().With<MoreComplicatedClassThatCantBeFullySatisfied>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);

            var second = container.Get<ISecondInterface>();
            Assert.That(second, Is.Not.Null);

            Assert.That(() => container.Get<IThirdInterface>(), Throws.InstanceOf<UnableToConstructException>());
        }

        [Test]
        public void TestExceptionThatIsThrowHasHierarchyInformationIfCantFullySatisfyConstructor()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().With<EmptyClassWithDefaultConstructor>();
            registry.Register<ISecondInterface>().With<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            registry.Register<IThirdInterface>().With<MoreComplicatedClassThatCantBeFullySatisfied>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);

            var second = container.Get<ISecondInterface>();
            Assert.That(second, Is.Not.Null);

            try {
                container.Get<IThirdInterface>();
            } catch (UnableToConstructException ex) {
                Assert.That(ex.AttemptedConstructors, Is.Not.Null);
                Assert.That(ex.AttemptedConstructors, Has.Count.EqualTo(1));

                Assert.That(ex.AttemptedConstructors[0].Parameters, Is.Not.Null);            
                Assert.That(ex.AttemptedConstructors[0].Parameters, Has.Count.EqualTo(2));
                Assert.That(ex.AttemptedConstructors[0].Success, Is.Not.True);

                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts, Is.Not.Null);
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts, Has.Count.EqualTo(1));

                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].Outcome, Is.EqualTo(ConstructionOutcome.Success));
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].Injected, Is.EqualTo(typeof(EmptyClassWithThatOneSimpleObjectInItsConstructor)));
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].Declared, Is.EqualTo(typeof(ISecondInterface)));           
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters, Is.Not.Null);
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters, Has.Count.EqualTo(1));
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Success, Is.EqualTo(ConstructionOutcome.Success));

                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters.ToList()[0].Injected, Is.EqualTo(typeof(EmptyClassWithDefaultConstructor)));
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters.ToList()[0].Declared, Is.EqualTo(typeof(ISimpleInterface)));

                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters.ToList()[0].ConstructorAttempts, Is.Not.Null);
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters.ToList()[0].ConstructorAttempts, Has.Count.EqualTo(1));
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters.ToList()[0].ConstructorAttempts[0].Success, Is.EqualTo(ConstructionOutcome.Success));
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters, Is.Not.Null);
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters.ToList()[0].ConstructorAttempts[0].Parameters, Is.Empty);

                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[1].Injected, Is.Null);
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[1].Declared, Is.EqualTo(typeof(IFourthInterfaceActuallyDoesntHaveAnyDerivedClasses)));
                Assert.That(ex.AttemptedConstructors[0].Parameters.ToList()[1].Outcome, Is.EqualTo(ConstructionOutcome.NoMappingFound));

            }
        }


        [Test]
        public void TestRegistryIsFoundAutomatically()
        {
            var builder = Clean.MakeBuilder();
            var container = builder.Container;

            var first = container.Get<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);

            var second = container.Get<ISecondInterface>();
            Assert.That(second, Is.Not.Null);
        }
    }
}