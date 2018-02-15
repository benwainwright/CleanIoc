namespace CleanIoc.Core.Interfaces
{
    using System.Collections.Generic;

    public interface IConstructorAttempt
    {
        IEnumerable<IInjectedType> Parameters { get; }

        bool Success { get; }
    }
}
