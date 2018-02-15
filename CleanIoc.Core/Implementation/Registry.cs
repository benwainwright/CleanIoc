/*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
* INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
* PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
* FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
* OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
namespace CleanIoc.Core
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Implementation;
    using CleanIoc.Core.Interfaces;

    public abstract class Registry : IRegistry, IMappable
    {
        private readonly Dictionary<Type, List<IRegistration>> types = new Dictionary<Type, List<IRegistration>>();

        private Type lastFromType;

        public IEnumerable<KeyValuePair<Type, List<IRegistration>>> Registrations => types;

        private IConstructorSelectionStrategy Strategy { get; set; }

        public void ConstructorStrategy(IConstructorSelectionStrategy strategy)
        {
            Strategy = strategy;
        }

        public IMappable Register<TFrom>(InstanceLifetime lifetime = InstanceLifetime.Singleton)
        {
            lastFromType = typeof(TFrom);
            return this;
        }

        public IRegistration WithConcreteType<TTo>()
        {
            if (!types.ContainsKey(lastFromType)) {
                types[lastFromType] = new List<IRegistration>();
            }

            var registration = new Registration(lastFromType, typeof(TTo));
            types[lastFromType].Add(registration);
            return registration;
        }
    }
}
