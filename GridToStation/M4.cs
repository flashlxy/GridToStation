using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GridToStation
{
    public class M4
    {
        public M4() { }

        public static string style = "4";

        string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        string y;

        public string Y
        {
            get { return y; }
            set { y = value; }
        }
        string m;

        public string M
        {
            get { return m; }
            set { m = value; }
        }
        string d;

        public string D
        {
            get { return d; }
            set { d = value; }
        }
        string h;

        public string H
        {
            get { return h; }
            set { h = value; }
        }
        string fore;

        public string Fore
        {
            get { return fore; }
            set { fore = value; }
        }
        string ceng;

        public string Ceng
        {
            get { return ceng; }
            set { ceng = value; }
        }
        double xDelt;

        public double XDelt
        {
            get { return xDelt; }
            set { xDelt = value; }
        }
        double yDelt;

        public double YDelt
        {
            get { return yDelt; }
            set { yDelt = value; }
        }
        double fJd;

        public double FJd
        {
            get { return fJd; }
            set { fJd = value; }
        }
        double eJd;

        public double EJd
        {
            get { return eJd; }
            set { eJd = value; }
        }
        double fWd;

        public double FWd
        {
            get { return fWd; }
            set { fWd = value; }
        }
        double eWd;

        public double EWd
        {
            get { return eWd; }
            set { eWd = value; }
        }
        int xSum;

        public int XSum
        {
            get { return xSum; }
            set { xSum = value; }
        }
        int ySum;

        public int YSum
        {
            get { return ySum; }
            set { ySum = value; }
        }
        double distance;

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        double minValue;

        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }
        double maxValue;

        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        int def1 = 0, def2 = 0;

        public int Def2
        {
            get { return def2; }
            set { def2 = value; }
        }

        public int Def1
        {
            get { return def1; }
            set { def1 = value; }
        }

        double[,] dataValue = null;

        public double[,] DataValue
        {
            get { return dataValue; }
            set { dataValue = value; }
        }

        double[,] RdataValue = null;

        public double[,] RDataValue
        {
            get { return RdataValue; }
            set { RdataValue = value; }
        }

       

        public static M4 ReadMicaps4(string filename)
        {
            M4 m4 = new M4();
            using (StreamReader sr = new StreamReader(filename, Encoding.Default))
            {
                string s = sr.ReadToEnd();

                string[] ss = s.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss == null || ss.Length < 2 || (ss[0] != "micaps" && ss[1] != M4.style)) return m4;
                if (ss.Length < 22) return m4;

                m4.Title = ss[2];
                m4.Y = ss[3];
                m4.M = ss[4];
                m4.D = ss[5];
                m4.H = ss[6];
                m4.Fore = ss[7];
                m4.Ceng = ss[8];
                m4.XDelt = double.Parse(ss[9]);
                m4.YDelt = double.Parse(ss[10]);
                m4.FJd = double.Parse(ss[11]);
                m4.EJd = double.Parse(ss[12]);
                m4.FWd = double.Parse(ss[13]);
                m4.EWd = double.Parse(ss[14]);
                m4.XSum = int.Parse(ss[15]);
                m4.YSum = int.Parse(ss[16]);
                m4.Distance = double.Parse(ss[17]);
                m4.MinValue = double.Parse(ss[18]);
                m4.MaxValue = double.Parse(ss[19]);


                //m4.Def1 = int.Parse(ss[20]);
                //m4.Def1 = int.Parse(ss[21]);


                //double XDelt = 0;
                //double YDelt = 0;

                //---- Generate X and Y coordinates            
                //_X = new double[m4.XSum];
                //_Y = new double[m4.YSum];

                //for (int i = 0; i < m4.XSum; i++)
                //{
                //    _X[i] = i * m4.XDelt;
                //}
                //for (int i = 0; i < m4.YSum; i++)
                //{
                //    _Y[i] = i * m4.YDelt;
                //}
                m4.RDataValue = new double[m4.YSum, m4.XSum];

                for (int i = 0; i < m4.YSum; i++)
                {
                    for (int j = 0; j < m4.XSum; j++)
                    {
                        m4.RDataValue[i, j] = m4.YDelt > 0 ? double.Parse(ss[22 + i * m4.XSum + j]) : double.Parse(ss[22 + (m4.YSum - 1 - i) * m4.XSum + j]);
                    }
                }
                m4.DataValue = new double[m4.YSum, m4.XSum];

                for (int i = 0; i < m4.YSum; i++)
                {
                    for (int j = 0; j < m4.XSum; j++)
                    {
                        m4.DataValue[i, j] = double.Parse(ss[22 + i * m4.XSum + j]);
                    }
                }


            }
            return m4;
        }


    }
}
