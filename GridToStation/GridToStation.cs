using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;


namespace GridToStation
{
    public class GridToStation
    {

        List<_STATION> stations = new List<_STATION>();

        public List<_STATION> Stations
        {
            get { return stations; }
            set { stations = value; }
        }

        double mvalue = 9999.0;
        bool linear;
        public GridToStation(bool linear, M4 m4)
        {
            this.linear = linear;
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"zdzstation.txt");
            stations = GetStations(filename);
            Interpolation(m4);
        }

        public GridToStation(string sta_File, string mic_File)
        {
            this.linear = true;
            stations = GetStations(sta_File);
            M4 m4 = M4.ReadMicaps4(mic_File);
            Interpolation(m4);
        }

        public GridToStation(string mic_File)
        {
            this.linear = true;
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"zdzstation.txt");
            stations = GetStations(filename);
            M4 m4 = M4.ReadMicaps4(mic_File);
            Interpolation(m4);
        }
        
        public double this[string code]
        {
            get { return GetValue(code); }
        }

        private double GetValue(string code)
        {
            foreach (_STATION sta in stations)
            {
                if (code == sta.code) return sta.svalue;
            }
            return mvalue;
        }

        private List<_STATION> GetStations(string filename)
        {
            List<_STATION> lstStations = new List<_STATION>();

            //string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"zdzstation.txt");

            if (File.Exists(filename))
            {
                try
                {
                    char[] spaceArray = new char[] { ' ', '\r', '\n', '\t' };
                    FileStream aFile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using (StreamReader sr = new StreamReader(aFile, Encoding.GetEncoding("GB18030")))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string input = sr.ReadLine();
                            string[] result = input.Split(spaceArray, StringSplitOptions.RemoveEmptyEntries);
                            int resultsum = result.GetUpperBound(0);
                            if (result.Length == 5)
                            {
                                _STATION sta = new _STATION();
                                sta.code = result[0];
                                sta.jd = double.Parse(result[1]);
                                sta.wd = double.Parse(result[2]);
                                sta.name = result[3];
                                sta.height = result[4];
                                lstStations.Add(sta);
                            }
                        }
                    }
                    aFile.Close();
                }
                catch { }
                finally { }
            }
            return lstStations;
        }

        private int fix(double o)
        {
            int i;
            try
            {
                i = int.Parse(Math.Floor(o).ToString("F0"));
                //i = int.Parse(o.ToString("F0"));
                return i;
            }
            catch
            {
                return 0;
            }

        }

        private static bool DoubleEquals(double a, double b)
        {
            return Math.Abs(a - b) < 10e-5;
        }

        private void Interpolation(M4 m4)
        {
            double xi, yi, a, b, c, d, sum, x1val, x2val;
            int xidx, yidx, i1, j1, i2, j2, n;
            double[] xarray = new double[m4.XSum];
            double[] yarray = new double[m4.YSum];

            double[] conc = new double[Stations.Count];

            for (int i = 0; i < m4.XSum; i++)
            {
                xarray[i] = m4.FJd + i * m4.XDelt;
            }
            for (int i = 0; i < m4.YSum; i++)
            {
                yarray[i] = (m4.YDelt > 0 ? m4.FWd : m4.EWd) + Math.Abs(i * m4.YDelt);
            }

            for (int i = 0; i < Stations.Count; i++)
            {
                conc[i] = mvalue;
                xi = (Stations[i].jd - xarray[0]) / m4.XDelt + 1.0;
                yi = (Stations[i].wd - yarray[0]) / Math.Abs(m4.YDelt) + 1.0;
                xidx = fix(xi);
                yidx = fix(yi);

                if (xidx >= 1 && xidx < m4.XSum && yidx >= 1 && yidx < m4.YSum)
                {
                    if (linear)
                    {
                        i1 = fix(yi);
                        j1 = fix(xi);
                        i2 = i1 + 1;
                        j2 = j1 + 1;
                        a = m4.RDataValue[i1 - 1, j1 - 1];
                        b = m4.RDataValue[i1 - 1, j2 - 1];
                        c = m4.RDataValue[i2 - 1, j1 - 1];
                        d = m4.RDataValue[i2 - 1, j2 - 1];

                        n = 0;
                        sum = 0.0;
                        if (!DoubleEquals(a, mvalue))
                        {
                            sum = sum + a;
                            n = n + 1;
                        }

                        if (!DoubleEquals(b, mvalue))
                        {
                            sum = sum + b;
                            n = n + 1;
                        }

                        if (!DoubleEquals(c, mvalue))
                        {
                            sum = sum + c;
                            n = n + 1;
                        }

                        if (!DoubleEquals(d, mvalue))
                        {
                            sum = sum + d;
                            n = n + 1;
                        }

                        if (n == 0)
                        {
                            continue;// cycle
                        }
                        else if (n <= 3)
                        {
                            conc[i] = sum / n;
                        }
                        else
                        {
                            x1val = a + (c - a) * (Stations[i].wd - yarray[i1-1]) / Math.Abs(m4.YDelt);
                            x2val = b + (d - b) * (Stations[i].wd - yarray[i1-1]) / Math.Abs(m4.YDelt);
                            conc[i] = x1val + (x2val - x1val) * (Stations[i].jd - xarray[j1-1]) / m4.XDelt;
                        }
                    }
                    else
                    {
                        i1 = Convert.ToInt32(Math.Round(yi));
                        j1 = Convert.ToInt32(Math.Round(xi));
                        conc[i] = m4.RDataValue[i1 - 1, j1 - 1];
                    }
                }
                stations[i].svalue = conc[i];
            }
        }
        
    }
}
