/*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
* INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
* PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
* FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
* OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
namespace CleanIoc.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class CleanIocException : Exception
    {
        public CleanIocException()
        {
        }

        public CleanIocException(string message)
            : base(message)
        {
        }

        public CleanIocException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CleanIocException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
