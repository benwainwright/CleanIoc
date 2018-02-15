namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Utils;

    internal class Container : ICleanIocContainer, IServiceProvider
    {
        public bool Initialised { get; private set; } = false;

        private List<ITypeRegistry> Registries { get; set; }

        private TypeRepository Repository { get; set; } = new TypeRepository();

        public IServiceProvider ServiceProvider { get { return this;  } }

        internal Container(List<ITypeRegistry> registries)
        {
            Guard.Against.Null(registries, nameof(registries));
            Registries = registries;
        }

        private void Initialise()
        {
            foreach (var registry in Registries) {
                Repository.AddRegistryContents(registry);
            }
        }

        public object GetService(Type from)
        {
            var instances = InitialiseAndGetInstances(from);
            if (instances.Count == 0) {
                return null;
            }
            return instances[0];
        }

        public T Get<T>()
            where T : class
        {
            var returnVal = GetAll<T>();
            if(returnVal.Count == 0) {
                return null;
            }
            return returnVal[0];
        }

        public IList<T> GetAll<T>()
            where T : class
        {
            Type from = typeof(T);
            var returnVal = new List<T>();
            var instances = InitialiseAndGetInstances(from);
            foreach(object instance in instances) {
                returnVal.Add(instance as T);
            }
            return returnVal;
        }

        private IList<object> InitialiseAndGetInstances(Type from)
        {
            if (!Initialised) {
                Initialise();
                Initialised = true;
            }
            return Repository.GetInstances(from, out List<IInjectedType> failed);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    Repository.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
