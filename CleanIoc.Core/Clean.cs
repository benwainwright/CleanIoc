namespace CleanIoc.Core
{
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Implementation;
    using CleanIoc.Core.Interfaces;

    public static class Clean
    {
        public static ScanBehaviour DefaultScanBehaviour { get; set; } = ScanBehaviour.ScanLoadedAssembliesForRegistries;

        public static IContainerBuilder MakeBuilder()
        {
            return new ContainerBuilder(DefaultScanBehaviour);
        }
    }
}
