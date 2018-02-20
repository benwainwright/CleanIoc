namespace CleanIoc.Core.Implementation.Loading
{
    using System.Linq;
    using System.Reflection;
    using CleanIoc.Core.Interfaces;
    using MetadataScanner;

    public class RegistryAssemblyLoader : IAssemblyLoader
    {
        private readonly string basePath;

        public RegistryAssemblyLoader(string basePath)
        {
            this.basePath = basePath;
        }

        public void Load()
        {
            var scanner = AssemblyScanner.Create(basePath);
            scanner.Scan();

            var assemblies = from assembly
                             in scanner.Assemblies
                             where (from type
                                    in assembly.TypeDefinitions
                                    where type.MatchesReflectionType(typeof(IRegistry))
                                    select type).Any()
                             select assembly;

            foreach (var assembly in assemblies) {
                Assembly.LoadFrom(assembly.FilePath);
            }
        }
    }
}
