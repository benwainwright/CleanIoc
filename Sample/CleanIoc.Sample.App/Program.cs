namespace CleanIoc.Sample.App
{
    using System;
    using CleanIoc.Core;
    using CleanIoc.Sample.Interfaces;

    internal class Program
    {
        public static void Main()
        {
            var builder = Clean.MakeBuilder();
            var container = builder.Container;
            var instance = container.GetInstanceOf<IMySimpleSampleInterface>();

            Console.WriteLine($"The message is '{instance.Message}'");
        }
    }
}
