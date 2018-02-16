/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
namespace CleanIoc.Core.Test
{
    using System.Linq;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Exceptions;
    using CleanIoc.Core.Test.DummyTypes.Concrete;
    using CleanIoc.Core.Test.DummyTypes.Interfaces;
    using CleanIoc.Core.Test.DummyTypes.Registries;
    using NUnit.Framework;

    [TestFixture]
    public class TestContainer
    {
        [Test]
        public void TestThatItWorksWithASimpleTypeWithADefaultConstructor()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().WithConcreteType<EmptyClassWithDefaultConstructor>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var instance = container.GetInstanceOf<ISimpleInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.TypeOf<EmptyClassWithDefaultConstructor>());
        }

        [Test]
        public void TestThatWithoutSettingALifetimeItIsTransient()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().WithConcreteType<EmptyClassWithDefaultConstructor>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.GetInstanceOf<ISimpleInterface>();
            var second = container.GetInstanceOf<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);
            Assert.That(first, Is.Not.SameAs(second));
        }

        [Test]
        public void TestThatItCanConstructTypesWithASingleRegistredType()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().WithConcreteType<EmptyClassWithDefaultConstructor>();
            registry.Register<ISecondInterface>().WithConcreteType<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.GetInstanceOf<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);
            Assert.That(first, Is.TypeOf<EmptyClassWithDefaultConstructor>());

            var second = container.GetInstanceOf<ISecondInterface>();
            Assert.That(second, Is.Not.Null);
            Assert.That(second, Is.TypeOf<EmptyClassWithThatOneSimpleObjectInItsConstructor>());
            Assert.That(second.FirstParam, Is.Not.Null);
            Assert.That(second.FirstParam, Is.InstanceOf<EmptyClassWithDefaultConstructor>());
        }

        [Test]
        public void TestThrowsAnExceptionIfCantFullySatisfyConstructor()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().WithConcreteType<EmptyClassWithDefaultConstructor>();
            registry.Register<ISecondInterface>().WithConcreteType<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            registry.Register<IThirdInterface>().WithConcreteType<MoreComplicatedClassThatCantBeFullySatisfied>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.GetInstanceOf<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);

            var second = container.GetInstanceOf<ISecondInterface>();
            Assert.That(second, Is.Not.Null);

            Assert.That(() => container.GetInstanceOf<IThirdInterface>(), Throws.InstanceOf<UnableToConstructException>());
        }

        [Test]
        public void TestExceptionThatIsThrowHasHierarchyInformationIfCantFullySatisfyConstructor()
        {
            var registry = new SimpleTypeRegistry();
            registry.Register<ISimpleInterface>().WithConcreteType<EmptyClassWithDefaultConstructor>();
            registry.Register<ISecondInterface>().WithConcreteType<EmptyClassWithThatOneSimpleObjectInItsConstructor>();
            registry.Register<IThirdInterface>().WithConcreteType<MoreComplicatedClassThatCantBeFullySatisfied>();
            var builder = Clean.MakeBuilder();
            builder.AddRegistry(registry);
            var container = builder.Container;

            var first = container.GetInstanceOf<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);

            var second = container.GetInstanceOf<ISecondInterface>();
            Assert.That(second, Is.Not.Null);

            try {
                container.GetInstanceOf<IThirdInterface>();
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

            var first = container.GetInstanceOf<ISimpleInterface>();
            Assert.That(first, Is.Not.Null);

            var second = container.GetInstanceOf<ISecondInterface>();
            Assert.That(second, Is.Not.Null);
        }
    }
}
