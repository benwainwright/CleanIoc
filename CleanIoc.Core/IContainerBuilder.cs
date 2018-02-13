namespace CleanIoc.Core
{
    using CleanIoc.Core.Interfaces;

    public interface IContainerBuilder
    {
        void AddRegistry(TypeRegistry registry);

        IContainer Container { get; }
    }
}
