namespace CleanIoc.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Interfaces;

    public interface ITypeMap
    {
        IConstructorSelectionStrategy ConstructorSelector { get; }

        InstanceLifetime Lifetime { get; set; }

        int Size { get; }

        IEnumerable<Type> Types { get; }

        void Add(Type type);

        void AddLoadedInstanceOf(Type type, object instance);

        object GetLoadedInstanceOf(Type type);
    }
}