namespace CleanIoc.Core.Interfaces
{
    using System;
    using CleanIoc.Core.Enums;

    public interface ITypeRegistration
    {
        Type From { get; }

        Lifetime Lifetime { get; }

        Type To { get; }

        void AsSingleton();

        void AsTransient();
    }
}