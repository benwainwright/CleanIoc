﻿/*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
* INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
* PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
* FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
* OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
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
