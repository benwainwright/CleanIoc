/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
namespace CleanIoc.Core.Implementation.Construction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Utils;

    internal class ConstructorAttempt : IConstructorAttempt, IExecutableConstructor
    {
        private Type type;

        private IList<ConstructionPlan> plans;

        public ConstructorAttempt(Type type, List<ConstructionPlan> parameters, bool success)
        {
            Guard.Against.Null(type, nameof(type));
            Guard.Against.Null(parameters, nameof(parameters));

            plans = parameters;
            Success = success;
            this.type = type;
        }

        public IEnumerable<IInjectedType> Parameters => plans;

        public bool Success { get; }

        public object Execute()
        {
            var values = from plan
                         in plans
                         select plan.GetInstance();

            return Activator.CreateInstance(type, values.ToArray());
        }
    }
}
