using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridToStation
{
    public class _STATION
    {
        public string code { get; set; }
        public string name { get; set; }
        public double jd { get; set; }
        public double wd { get; set; }
        public string height { get; set; }
        public double svalue { get; set; }
        public _STATION()
        {
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(code + " " + jd + " " + wd + " " + svalue.ToString("F1"));
            return sb.ToString();
        }
    }
}
