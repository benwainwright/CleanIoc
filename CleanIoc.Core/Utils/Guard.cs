namespace CleanIoc.Core.Utils
{
    using System;

    public static class Guard
    {
        public static class Against
        {
            public static void Null(object value, string argument)
            {
                if(argument == null){
                    throw new ArgumentNullException($"{argument} cannot be null!");
                }
            }
        }
    }
}
