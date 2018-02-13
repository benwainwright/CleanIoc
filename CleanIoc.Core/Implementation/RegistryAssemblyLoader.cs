namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using CleanIoc.Core.Interfaces;

    public class RegistryAssemblyLoader : IAssemblyLoader
    {
        public void Load()
        {
            var assembliesToScan = new List<string>();
            var domain = AppDomain.CurrentDomain;
            var domainSetup = domain.SetupInformation;

            if(!domainSetup.DisallowApplicationBaseProbing) {
                if(domainSetup.PrivateBinPathProbe == null) {
                    assembliesToScan.AddRange(GetNotLoadedAssemblyNames(domain.BaseDirectory));
                }
                assembliesToScan.AddRange(GetNotLoadedAssemblyNames(domain.RelativeSearchPath));
            }
            LoadAssembliesContainingRegistries(assembliesToScan);
        }

        private List<string> GetNotLoadedAssemblyNames(string dir)
        {
            var names = new List<string>();
            var currentlyLoadedAssemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            var files = Directory.EnumerateFiles(dir);

            foreach(string file in files) {
                string name = AssemblyName.GetAssemblyName(file).FullName;
                if (!currentlyLoadedAssemblies.Exists(a => a.FullName.Equals(name, StringComparison.InvariantCultureIgnoreCase))) {
                    names.Add(name);
                }
            }
            return names;
        }

        private void LoadAssembliesContainingRegistries(List<string> names)
        {
            foreach(string name in names) {
                var assembly = Assembly.ReflectionOnlyLoad(name);
                foreach(var type in assembly.GetExportedTypes()) {
                    if(typeof(ITypeRepository).IsAssignableFrom(type)) {
                        Assembly.Load(name);
                    }
                }
            }
        }
    }
}
