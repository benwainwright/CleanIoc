using System;
using CleanIoc.Core.Enums;

namespace CleanIoc.Core.Interfaces
{
    public interface ITypeRegistration
    {
        Type From { get; }

        Lifetime Lifetime { get; }

        Type To { get; }

        void AsSingleton();

        void AsTransient();
    }
}