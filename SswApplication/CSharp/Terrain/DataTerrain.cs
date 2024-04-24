using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SswApplication.CSharp.Terrain
{
    internal class DataTerrain
    {
        public static List<int> X_relief(int stop)
        {
            List<int> res = [];
            for (int i = 0; i<stop; i++) {
                res.Add(i);
            }
            return res;
        }

        public static List<double> Z_relief(string path)
        {
            List<double> res = [];
            string[][] data = FileFunctions.ReadCSV(path);
            foreach (string[] dataArray in data)
            {
                res.Add(Double.Parse(dataArray[0], CultureInfo.InvariantCulture));
            }
            return res;
        }
    }
}
