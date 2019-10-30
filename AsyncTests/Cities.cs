using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncTests
{
    public class Cities
    {
        public static List<string> CityNames = new List<string>() {"1", "2" , "3", "4", "5", "6" };

        public static void Initialize()
        {
            CityNames = new List<string>();
            for (int i = 0; i < 50; i++)
            {
                CityNames.Add(i.ToString());
            }
        }
    }
}
