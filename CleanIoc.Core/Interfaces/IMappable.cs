namespace CleanIoc.Core.Interfaces
{
    public interface IMappable
    {
        ITypeRegistration With<TTo>();
    }
}
