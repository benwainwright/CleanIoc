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
    using System.Reflection;
    using CleanIoc.Core.Interfaces;

    internal class DefaultConstructorSelectionStrategy : IConstructorSelectionStrategy
    {
        public ConstructorInfo SelectConstructor(Type type, IList<ConstructorInfo> remainingConstructors)
        {
            int max = -1;
            ConstructorInfo returnVal = null;
            foreach (var constructor in remainingConstructors) {
                var paramaters = constructor.GetParameters();
                if (paramaters.Length > max) {
                    returnVal = constructor;
                    max = paramaters.Length;
                }
            }

            return returnVal;
        }
    }
}
