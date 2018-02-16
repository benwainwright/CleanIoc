/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
namespace CleanIoc.Sample.App
{
    using System;
    using CleanIoc.Core;
    using CleanIoc.Sample.Interfaces;

    internal class Program
    {
        public static void Main()
        {
            var builder = Clean.MakeBuilder();
            var container = builder.Container;
            var instance = container.GetInstanceOf<IMySimpleSampleInterface>();

            Console.WriteLine($"The message is '{instance.Message}'");
        }
    }
}
