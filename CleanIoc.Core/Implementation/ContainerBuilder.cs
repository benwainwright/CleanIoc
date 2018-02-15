namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;

    internal class ContainerBuilder : IContainerBuilder, IDisposable
    {
        private readonly ScanBehaviour scanBehaviour;

        private readonly List<ITypeRegistry> registries = new List<ITypeRegistry>();

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

        public void AddRegistry(ITypeRegistry registry)
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

        private static List<ITypeRegistry> GetRegistries()
        {
            var registries = new List<ITypeRegistry>();
            var registryType = typeof(TypeRegistry);
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in loadedAssemblies) {
                var types = assembly.GetExportedTypes();
                foreach (var type in types) {
                    if (registryType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface) {
                        var registry = Activator.CreateInstance(type) as TypeRegistry;
                        registries.Add(registry);
                    }
                }
            }

            return registries;
        }

        private void LoadAssemblies()
        {
            if (loaders.Count == 0) {
                loaders.Add(new RegistryAssemblyLoader());
            }

            foreach (var loader in loaders) {
                loader.Load();
            }
        }
    }
}
