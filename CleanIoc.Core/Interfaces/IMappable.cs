namespace CleanIoc.Core.Interfaces
{
    public interface IMappable
    {
        IRegistration WithConcreteType<TTo>();
    }
}
