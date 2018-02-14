namespace CleanIoc.Core.Implementation
{
    using System;
    using System.Collections.Generic;
    using CleanIoc.Core.Interfaces;

    public class Container : IContainer
    {
        public bool Initialised { get; private set; } = false;

        private List<ITypeRegistry> Registries { get; set; }

        private TypeRepository Repository { get; set; } = new TypeRepository();

        internal Container(List<ITypeRegistry> registries)
        {
            Registries = registries;
        }

        private void Initialise()
        {
            foreach (var registry in Registries) {
                Repository.AddRegistryContents(registry);
            }
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
            if(!Initialised) {
                Initialise();
                Initialised = true;
            }
            Type from = typeof(T);
            var returnVal = new List<T>();
            var instances = Repository.GetInstances(from);
            foreach(object instance in instances) {
                returnVal.Add(instance as T);
            }
            return returnVal;
        }
    }
}
