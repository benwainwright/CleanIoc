namespace CleanIoc.Core.Implementation
{
    using System;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Utils;

    internal class TypeRegistration : ITypeRegistration
    {
        public Type From { get; }

        public Type To { get; }

        public Lifetime Lifetime { get; set; } = Lifetime.Transient;

        public TypeRegistration(Type from, Type to)
        {
            Guard.Against.Null(from, nameof(from));
            Guard.Against.Null(to, nameof(to));

            From = from;
            To = to;
        }

        public void AsSingleton()
        {
            Lifetime = Lifetime.Singleton;
        }

        public void AsTransient()
        {
            Lifetime = Lifetime.Transient;
        }
    }
}
