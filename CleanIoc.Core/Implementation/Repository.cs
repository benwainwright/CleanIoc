/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Exceptions;
    using CleanIoc.Core.Implementation.Construction;
    using CleanIoc.Core.Interfaces;

    internal class Repository : IRepository
    {
        private readonly Dictionary<Type, HashSet<ConstructionPlan>> allPlans = new Dictionary<Type, HashSet<ConstructionPlan>>();

        private readonly IConstructorSelectionStrategy constructorSelectionStrategy;

        private bool disposed = false;

        public Repository(IConstructorSelectionStrategy constructorSelectionStrategy = null)
        {
            this.constructorSelectionStrategy = constructorSelectionStrategy ?? new DefaultConstructorSelectionStrategy();
        }

        public void AddRegistryContents(IRegistry registry)
        {
            foreach (var entry in registry.Registrations) {
                foreach (var toType in entry.Value) {
                    AddRegistration(toType);
                }
            }
        }

        public void AddRegistration(IRegistration registration)
        {
            if (registration == null) {
                throw new ArgumentNullException($"{nameof(registration)} cannot be null");
            }

            if (!allPlans.ContainsKey(registration.DefinedType)) {
                allPlans[registration.DefinedType] = new HashSet<ConstructionPlan>();
            }

            allPlans[registration.DefinedType].Add(new ConstructionPlan(registration, allPlans, constructorSelectionStrategy));
        }

        public IList<object> GetInstances(Type from, out List<IInjectedType> failed)
        {
            var returnVal = new List<object>();
            failed = new List<IInjectedType>();
            HashSet<ConstructionPlan> plans;
            try {
                plans = allPlans[from];
            } catch (KeyNotFoundException ex) {
                throw new MappingNotFoundException($"No mapping was found for type {from.FullName}", ex);
            }

            foreach (var plan in plans) {
                if (plan.CanBeConstructed()) {
                    returnVal.Add(plan.GetInstance());
                } else {
                    failed.Add(plan);
                }
            }

            return returnVal;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // TODO - dispose of disposable singletons
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed) {
                if (disposing) {
                }

                disposed = true;
            }
        }
    }
}