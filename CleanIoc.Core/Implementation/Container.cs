namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Interfaces;
    using CleanIoc.Core.Utils;

    internal class Container : ICleanIocContainer, IServiceProvider
    {
        private List<IRegistry> registries;

        private Repository repository = new Repository();

        private bool disposed = false;

        internal Container(List<IRegistry> registries)
        {
            Guard.Against.Null(registries, nameof(registries));
            this.registries = registries;
        }

        public IServiceProvider ServiceProvider => this;

        public bool Initialised { get; private set; } = false;

        public object GetService(Type from)
        {
            var instances = InitialiseAndGetInstances(from);
            if (instances.Count == 0) {
                return null;
            }

            return instances[0];
        }

        public T GetInstanceOf<T>()
            where T : class
        {
            var returnVal = GetAll<T>();
            if (returnVal.Count == 0) {
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
            foreach (object instance in instances) {
                returnVal.Add(instance as T);
            }

            return returnVal;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed) {
                if (disposing) {
                    repository.Dispose();
                }

                disposed = true;
            }
        }

        private IList<object> InitialiseAndGetInstances(Type from)
        {
            if (!Initialised) {
                Initialise();
                Initialised = true;
            }

            return repository.GetInstances(from, out List<IInjectedType> failed);
        }

        private void Initialise()
        {
            foreach (var registry in registries) {
                repository.AddRegistryContents(registry);
            }
        }
    }
}
