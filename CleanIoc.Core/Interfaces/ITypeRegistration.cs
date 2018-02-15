namespace CleanIoc.Core.Interfaces
{
    using System;
    using CleanIoc.Core.Enums;

    public interface ITypeRegistration
    {
        Type DefinedType { get; }

        InstanceLifetime Lifetime { get; }

        Type InjectedType { get; }

        void AsSingleton();

        void AsTransient();
    }
}