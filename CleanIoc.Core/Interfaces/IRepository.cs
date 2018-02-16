﻿/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Implementation;

    public interface IRepository : IDisposable
    {
        void AddRegistryContents(IRegistry registry);

        void AddRegistration(IRegistration registration);

        IList<object> GetInstances(Type from, out List<IInjectedType> failed);
    }
}