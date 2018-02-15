namespace CleanIoc.Sample.Library
{
    using CleanIoc.Sample.Interfaces;

    public class MySimpleClass : IMySimpleSampleInterface
    {
        public string Message => "Hello!";
    }
}
