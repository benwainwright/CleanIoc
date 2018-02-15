namespace CleanIoc.Core.Implementation
{
    using System;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Utils;

    internal class TypeRegistration : ITypeRegistration
    {
        public TypeRegistration(Type from, Type to)
        {
            Guard.Against.Null(from, nameof(from));
            Guard.Against.Null(to, nameof(to));

            DefinedType = from;
            InjectedType = to;
        }

        public Type DefinedType { get; }

        public Type InjectedType { get; }

        public InstanceLifetime Lifetime { get; set; } = InstanceLifetime.Transient;

        public void AsSingleton()
        {
            Lifetime = InstanceLifetime.Singleton;
        }

        public void AsTransient()
        {
            Lifetime = InstanceLifetime.Transient;
        }
    }
}
