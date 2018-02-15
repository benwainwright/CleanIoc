using CleanIoc.Core.Enums;
using CleanIoc.Core.Exceptions;
using CleanIoc.Core.Implementation;
using CleanIoc.Core.Interfaces;
using CleanIoc.Core.Test.DummyTypes.Concrete;
using CleanIoc.Core.Test.DummyTypes.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanIoc.Core.Test.Implementation
{
    [TestFixture]
    public class TypeRepositoryTest
    {

        [Test]
        public void TestThatTypeRepositoryThrowsAnExceptionIfItIsUnableToSatisfyADependency()
        {
            var repository = new TypeRepository();

            var first = new TypeRegistration(typeof(ISimpleInterface), typeof(EmptyClassWithDefaultConstructor)) {
                Lifetime = Lifetime.Singleton
            };
            repository.AddRegistration(first);

            var second = new TypeRegistration(typeof(ISecondInterface), typeof(EmptyClassWithThatOneSimpleObjectInItsConstructor)) {
                Lifetime = Lifetime.Singleton
            };
            repository.AddRegistration(second);

            var third = new TypeRegistration(typeof(IThirdInterface), typeof(MoreComplicatedClassThatCantBeFullySatisfied)) {
                Lifetime = Lifetime.Singleton
            };
            repository.AddRegistration(third);


            var firstInstances = repository.GetInstances(typeof(ISimpleInterface));
            Assert.That(firstInstances, Is.Not.Null);
            Assert.That(firstInstances, Has.Count.EqualTo(1));
            Assert.That(firstInstances[0] as EmptyClassWithDefaultConstructor, Is.Not.Null);

            var secondInstances = repository.GetInstances(typeof(ISecondInterface));
            Assert.That(secondInstances, Is.Not.Null);
            Assert.That(secondInstances, Has.Count.EqualTo(1));
            Assert.That(secondInstances[0] as EmptyClassWithThatOneSimpleObjectInItsConstructor, Is.Not.Null);           

            Assert.That(() => repository.GetInstances(typeof(IThirdInterface)), Throws.InstanceOf<UnableToConstructException>());
        }

        [Test]
        public void TestThatTypeRepositoryCorrectlyReturnsInstanceForASimpleTypeRegistrationThatOnlyHasADefaultConstructor()
        {
            var registration = new TypeRegistration(typeof(ISimpleInterface), typeof(EmptyClassWithDefaultConstructor)) {
                Lifetime = Lifetime.Singleton
            };
            var repository = new TypeRepository();
            repository.AddRegistration(registration);

            var instances = repository.GetInstances(typeof(ISimpleInterface));

            Assert.That(instances, Is.Not.Null);
            Assert.That(instances, Has.Count.EqualTo(1));
            Assert.That(instances[0], Is.InstanceOf(typeof(EmptyClassWithDefaultConstructor)));
        }

        [Test]
        public void TestThatTypeRepositoryCorrectlyReturnsInstanceForATypeRegistrationThatOnlyHasAConstructorWithAsingleEasyDependency()
        {
            var repository = new TypeRepository();

            var first = new TypeRegistration(typeof(ISimpleInterface), typeof(EmptyClassWithDefaultConstructor)) {
                Lifetime = Lifetime.Singleton
            };
            repository.AddRegistration(first);

            var second = new TypeRegistration(typeof(ISecondInterface), typeof(EmptyClassWithThatOneSimpleObjectInItsConstructor)) {
                Lifetime = Lifetime.Singleton
            };
            repository.AddRegistration(second);
            
            var firstInstances = repository.GetInstances(typeof(ISimpleInterface));
            Assert.That(firstInstances, Is.Not.Null);
            Assert.That(firstInstances, Has.Count.EqualTo(1));
            Assert.That(firstInstances[0], Is.InstanceOf(typeof(EmptyClassWithDefaultConstructor)));

            var secondInstances = repository.GetInstances(typeof(ISecondInterface));
            Assert.That(secondInstances, Is.Not.Null);
            Assert.That(secondInstances, Has.Count.EqualTo(1));
            Assert.That(secondInstances[0], Is.InstanceOf(typeof(EmptyClassWithThatOneSimpleObjectInItsConstructor)));
            var secondInstance = secondInstances[0] as ISecondInterface;
            Assert.That(secondInstance.FirstParam, Is.Not.Null);
            Assert.That(secondInstance.FirstParam, Is.InstanceOf<EmptyClassWithDefaultConstructor>());
        }
    }
}
