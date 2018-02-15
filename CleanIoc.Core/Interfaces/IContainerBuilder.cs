namespace CleanIoc.Core.Interfaces
{
    public interface IContainerBuilder
    {
        ICleanIocContainer Container { get; }

        void AddRegistry(IRegistry registry);

        void AddAssemblyLoader(IAssemblyLoader loader);
    }
}
