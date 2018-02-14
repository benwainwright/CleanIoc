namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using CleanIoc.Core.Interfaces;

    internal class RegistryAssemblyLoader : IAssemblyLoader, IDisposable
    {
        public void Load()
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += AppDomain_ReflectionOnlyAssemblyResolve;
            var assembliesToScan = new List<string>();
            var domain = AppDomain.CurrentDomain;
            var domainSetup = domain.SetupInformation;

            if(!domainSetup.DisallowApplicationBaseProbing) {
                if(domainSetup.PrivateBinPathProbe == null) {
                    assembliesToScan.AddRange(GetNotLoadedAssemblyNames(domain.BaseDirectory));
                }
                if (domain.RelativeSearchPath != null) {
                    assembliesToScan.AddRange(GetNotLoadedAssemblyNames(domain.RelativeSearchPath));
                }
            }
            LoadAssembliesContainingRegistries(assembliesToScan);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= AppDomain_ReflectionOnlyAssemblyResolve;
        }

        private Assembly AppDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

        private List<string> GetNotLoadedAssemblyNames(string dir)
        {
            var names = new List<string>();
            var currentlyLoadedAssemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            var files = Directory.EnumerateFiles(dir);

            foreach(string file in files) {
                var parts = file.Split('.');
                if (parts.Length > 0 && parts[parts.Length - 1].Equals("dll", StringComparison.InvariantCultureIgnoreCase)) {
                    string name = AssemblyName.GetAssemblyName(file).FullName;
                    if (!currentlyLoadedAssemblies.Exists(a => a.FullName.Equals(name, StringComparison.InvariantCultureIgnoreCase))) {
                        names.Add(name);
                    }
                }
            }
            return names;
        }

        private void LoadAssembliesContainingRegistries(List<string> names)
        {
            foreach(string name in names) {
                var assembly = Assembly.ReflectionOnlyLoad(name);
                foreach(var type in assembly.GetExportedTypes()) {
                    if(TypeIsDerivedFromRegistryInterface(type)) {
                        Assembly.Load(name);
                    }
                }
            }
        }

        private bool TypeIsDerivedFromRegistryInterface(Type type)
        {
            var registryType = typeof(ITypeRegistry);
            // So .NET has this weird (in my opinion) design flaw - to inspect assemblies
            // before fully loading them, you need to load them in a 'reflection only'
            // context... except if you do this, the reflection 'Type' objects are instances
            // of System.ReflectionOnlyType instead of System.RuntimeType. This means
            // I can't do typeof(ITypeRegistry).IsAssignableFrom(type), so I have to
            // implement this hack instead...
            //
            // :-(
            foreach (var theInterface in type.GetInterfaces()) {
                if(theInterface.AssemblyQualifiedName.Equals(registryType.AssemblyQualifiedName, StringComparison.InvariantCultureIgnoreCase)) {
                    return true;
                }
            }
            return false;
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= AppDomain_ReflectionOnlyAssemblyResolve;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
