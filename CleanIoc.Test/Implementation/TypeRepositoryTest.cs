/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
namespace CleanIoc.Core.Test.Implementation
{
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Implementation;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Test.DummyTypes.Concrete;
    using CleanIoc.Core.Test.DummyTypes.Interfaces;
    using NUnit.Framework;

    [TestFixture]
    public class TypeRepositoryTest
    {
        [Test]
        public void TestThatTypeRepositoryReturnsNoInstanceAndAFailedPlanIfADependencyCannotBeSatisfied()
        {
            var repository = new Repository();

            var first = new Registration(typeof(ISimpleInterface), typeof(EmptyClassWithDefaultConstructor)) {
                Lifetime = InstanceLifetime.Singleton
            };
            repository.AddRegistration(first);

            var second = new Registration(typeof(ISecondInterface), typeof(EmptyClassWithThatOneSimpleObjectInItsConstructor)) {
                Lifetime = InstanceLifetime.Singleton
            };
            repository.AddRegistration(second);

            var third = new Registration(typeof(IThirdInterface), typeof(MoreComplicatedClassThatCantBeFullySatisfied)) {
                Lifetime = InstanceLifetime.Singleton
            };
            repository.AddRegistration(third);

            var firstInstances = repository.GetInstances(typeof(ISimpleInterface), out List<IInjectedType> failed);
            Assert.That(firstInstances, Is.Not.Null);
            Assert.That(firstInstances, Has.Count.EqualTo(1));
            Assert.That(firstInstances[0] as EmptyClassWithDefaultConstructor, Is.Not.Null);

            var secondInstances = repository.GetInstances(typeof(ISecondInterface), out failed);
            Assert.That(secondInstances, Is.Not.Null);
            Assert.That(secondInstances, Has.Count.EqualTo(1));
            Assert.That(secondInstances[0] as EmptyClassWithThatOneSimpleObjectInItsConstructor, Is.Not.Null);

            var finalInstances = repository.GetInstances(typeof(IThirdInterface), out failed);
            Assert.That(finalInstances, Is.Not.Null);
            Assert.That(finalInstances, Is.Empty);
            Assert.That(failed, Is.Not.Null);
            Assert.That(failed, Has.Count.EqualTo(1));
        }

        [Test]
        public void TestThatTypeRepositoryCorrectlyReturnsInstanceForASimpleTypeRegistrationThatOnlyHasADefaultConstructor()
        {
            var registration = new Registration(typeof(ISimpleInterface), typeof(EmptyClassWithDefaultConstructor)) {
                Lifetime = InstanceLifetime.Singleton
            };
            var repository = new Repository();
            repository.AddRegistration(registration);

            var instances = repository.GetInstances(typeof(ISimpleInterface), out List<IInjectedType> failed);

            Assert.That(instances, Is.Not.Null);
            Assert.That(instances, Has.Count.EqualTo(1));
            Assert.That(instances[0], Is.InstanceOf(typeof(EmptyClassWithDefaultConstructor)));
        }

        [Test]
        public void TestThatTypeRepositoryCorrectlyReturnsInstanceForATypeRegistrationThatOnlyHasAConstructorWithAsingleEasyDependency()
        {
            var repository = new Repository();

            var first = new Registration(typeof(ISimpleInterface), typeof(EmptyClassWithDefaultConstructor)) {
                Lifetime = InstanceLifetime.Singleton
            };
            repository.AddRegistration(first);

            var second = new Registration(typeof(ISecondInterface), typeof(EmptyClassWithThatOneSimpleObjectInItsConstructor)) {
                Lifetime = InstanceLifetime.Singleton
            };
            repository.AddRegistration(second);

            var firstInstances = repository.GetInstances(typeof(ISimpleInterface), out List<IInjectedType> failed);
            Assert.That(firstInstances, Is.Not.Null);
            Assert.That(firstInstances, Has.Count.EqualTo(1));
            Assert.That(firstInstances[0], Is.InstanceOf(typeof(EmptyClassWithDefaultConstructor)));

            var secondInstances = repository.GetInstances(typeof(ISecondInterface), out failed);
            Assert.That(secondInstances, Is.Not.Null);
            Assert.That(secondInstances, Has.Count.EqualTo(1));
            Assert.That(secondInstances[0], Is.InstanceOf(typeof(EmptyClassWithThatOneSimpleObjectInItsConstructor)));
            var secondInstance = secondInstances[0] as ISecondInterface;
            Assert.That(secondInstance.FirstParam, Is.Not.Null);
            Assert.That(secondInstance.FirstParam, Is.InstanceOf<EmptyClassWithDefaultConstructor>());
        }
    }
}
