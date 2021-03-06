﻿/*
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
    using System.IO;
    using System.Reflection;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Implementation.Loading;
    using CleanIoc.Core.Interfaces;
    using System.Linq;

    internal class ContainerBuilder : IContainerBuilder, IDisposable
    {
        private readonly ScanBehaviour scanBehaviour;

        private readonly List<IRegistry> registries = new List<IRegistry>();

        private readonly List<IAssemblyLoader> loaders = new List<IAssemblyLoader>();

        private bool disposed = false;

        private ICleanIocContainer container;

        public ContainerBuilder(ScanBehaviour scanBehaviour = ScanBehaviour.ScanLoadedAssembliesForRegistries)
        {
            this.scanBehaviour = scanBehaviour;
        }

        public ICleanIocContainer Container
        {
            get
            {
                if (container == null) {
                    LoadAssemblies();
                    if (scanBehaviour != ScanBehaviour.Off) {
                        registries.AddRange(GetRegistries());
                    }

                    container = new Container(registries);
                }

                return container;
            }
        }

        public void AddRegistry(IRegistry registry)
        {
            registries.Add(registry);
        }

        public void AddAssemblyLoader(IAssemblyLoader loader)
        {
            loaders.Add(loader);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed) {
                if (disposing) {
                }

                disposed = true;
            }
        }

        private static List<IRegistry> GetRegistries()
        {
            var registries = new List<IRegistry>();
            var registryType = typeof(Registry);

            var loadedAssemblies = from assembly
                                   in AppDomain.CurrentDomain.GetAssemblies()
                                   where !assembly.IsDynamic
                                   select assembly;

            foreach (var assembly in loadedAssemblies) {
                var types = assembly.GetExportedTypes();
                foreach (var type in types) {
                    if (registryType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface) {
                        var registry = Activator.CreateInstance(type) as Registry;
                        registries.Add(registry);
                    }
                }
            }

            return registries;
        }

        private void LoadAssemblies()
        {
            if (loaders.Count == 0) {
                var assemblyLocation = Assembly.GetEntryAssembly().Location;

                loaders.Add(new RegistryAssemblyLoader(Path.GetDirectoryName(assemblyLocation)));
            }

            foreach (var loader in loaders) {
                loader.Load();
            }
        }
    }
}
