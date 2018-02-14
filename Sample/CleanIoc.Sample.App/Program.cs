namespace CleanIoc.Sample.App
{
    using System;
    using CleanIoc.Core;
    using CleanIoc.Sample.Interfaces;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = Clean.MakeBuilder();
            var container = builder.Container;
            var instance = container.Get<IMySimpleSampleInterface>();

            Console.WriteLine(string.Format("The message is '{0}'", instance.Message));
        }
    }
}
