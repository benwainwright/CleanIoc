namespace CleanIoc.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public static class Guard
    {
        public static class Against
        {
            public static void Null(string argument)
            {
                if(argument == null){
                    throw new ArgumentNullException($"{argument} cannot be null!");
                }
            }
        }
    }
}
