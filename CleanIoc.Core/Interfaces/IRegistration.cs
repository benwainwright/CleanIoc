namespace CleanIoc.Core.Interfaces
{
    using System;
    using CleanIoc.Core.Enums;

    public interface IRegistration
    {
        Type DefinedType { get; }

        InstanceLifetime Lifetime { get; }

        Type InjectedType { get; }

        void AsSingleton();

        void AsTransient();
    }
}