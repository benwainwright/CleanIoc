namespace CleanIoc.Core.Interfaces
{
    public interface IMappable
    {
        ITypeRegistration WithConcreteType<TTo>();
    }
}
