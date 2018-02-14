namespace CleanIoc.Core
{
    using CleanIoc.Core.Enums;
    using CleanIoc.Core.Implementation;

    public static class Clean
    {
        public static ScanBehaviour DefaultScanBehaviour { get; set; } = ScanBehaviour.ScanLoadedAssembliesForRegistries;

        public static IContainerBuilder MakeBuilder()
        {
            return new ContainerBuilder(DefaultScanBehaviour);
        }
    }
}
