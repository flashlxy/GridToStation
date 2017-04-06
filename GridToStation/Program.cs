using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridToStation
{
    class Program
    {
        static void Main(string[] args)
        {
            string micaps_file = @"K:\ecmwf\height\500\16012708.240";
            GridToStation gs = new GridToStation(micaps_file);
            foreach (_STATION station in gs.Stations)
            {
                Console.WriteLine(station.ToString());
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
