namespace CleanIoc.Core.Interfaces
{
    public interface IContainerBuilder
    {
        ICleanIocContainer Container { get; }

        void AddRegistry(ITypeRegistry registry);

        void AddAssemblyLoader(IAssemblyLoader loader);
    }
}
