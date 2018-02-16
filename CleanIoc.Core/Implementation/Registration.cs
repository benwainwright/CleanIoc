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
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Utils;

    internal class Registration : IRegistration
    {
        public Registration(Type from, Type to)
        {
            Guard.Against.Null(from, nameof(from));
            Guard.Against.Null(to, nameof(to));

            DefinedType = from;
            InjectedType = to;
        }

        public Type DefinedType { get; }

        public Type InjectedType { get; }

        public InstanceLifetime Lifetime { get; set; } = InstanceLifetime.Transient;

        public void AsSingleton()
        {
            Lifetime = InstanceLifetime.Singleton;
        }

        public void AsTransient()
        {
            Lifetime = InstanceLifetime.Transient;
        }
    }
}
