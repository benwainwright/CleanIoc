namespace CleanIoc.Core.Interfaces
{
    public interface IContainerBuilder
    {
        void AddRegistry(ITypeRegistry registry);

        ICleanIocContainer Container { get; }

        void AddAssemblyLoader(IAssemblyLoader loader);
    }
}
