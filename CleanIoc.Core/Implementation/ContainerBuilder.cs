namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;

    public class ContainerBuilder : IContainerBuilder
    {
        private List<ITypeRegistry> Registries { get; } = new List<ITypeRegistry>();

        private List<IAssemblyLoader> Loaders { get; } = new List<IAssemblyLoader>();

        private readonly ScanBehaviour scanBehaviour;

        private IContainer container;

        public IContainer Container
        {
            get
            {
                if (container == null) {
                    LoadAssemblies();
                    if (scanBehaviour != ScanBehaviour.Off) {
                        Registries.AddRange(GetRegistries());
                    }
                    container = new Container(Registries);
                }
                return container;
            }
        }

        public ContainerBuilder(ScanBehaviour scanBehaviour = ScanBehaviour.ScanLoadedAssembliesForRegistries)
        {
            this.scanBehaviour = scanBehaviour;
        }

        private void LoadAssemblies()
        {
            if (Loaders.Count == 0) {
                Loaders.Add(new RegistryAssemblyLoader());
            }
            foreach(var loader in Loaders) {
                loader.Load();
            }
        }

        private List<ITypeRegistry> GetRegistries()
        {
            var registries = new List<ITypeRegistry>();
            var registryType = typeof(TypeRegistry);
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach(var assembly in loadedAssemblies) {
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

        public void AddRegistry(TypeRegistry registry)
        {
            Registries.Add(registry);
        }

        public void AddAssemblyLoader(IAssemblyLoader loader)
        {
            Loaders.Add(loader);
        }
    }
}
