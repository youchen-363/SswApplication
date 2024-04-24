using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Maui;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SswApplication.CSharp.Source
{
    internal class DataSource
    {

        public static string InitialiseDataSource()
        {
            // pas passe
            ConfigurationSource config = ConfigurationSource.ExtractOutputCSVSource(@"C:\ENAC\SswApplication\SswApplication\CodeSource\Source\outputs\configuration.csv");
            double[] efield_db = EFieldToEFieldDB(FileFunctions.ReadCSV(@"C:\ENAC\SswApplication\SswApplication\CodeSource\Source\outputs\E_field.csv"));
            double v_max = MaxInArray(efield_db);
            double v_min = MinInArray(v_max);
            double[] z_vect = GenerateValues(config, false);
            string json = SerializeToJson(v_min, v_max, efield_db, z_vect, config);
            return json;
        }

        static string SerializeToJson(double vmin, double vmax, double[] efielddb, double[] zvect, ConfigurationSource conf)
        {

            var data = new
            {
                v_min = vmin,
                v_max = vmax,
                efield_db = efielddb,
                z_vect = zvect,
                config = conf
            };

            // Serialize the data object to JSON
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        // to find v_max
        public static double MaxInArray(double[] arr)
        {
            return arr.Max(x => x);
        }

        // find v_min
        public static double MinInArray(double v_max)
        {
            return v_max - 100;
        }

        // for z_vect = np.linspace(0, ConfigSource.z_step*ConfigSource.n_z, num=ConfigSource.n_z, endpoint=False)
        public static double[] GenerateValues(ConfigurationSource config, bool endpoint)
        {
            double start = 0;
            double stop = config.Z_step.Value * config.N_z.Value;
            int num = (int)config.N_z.Value;

            double[] result = new double[num];
            double step = (stop - start) / (endpoint ? num - 1 : num);

            for (int i = 0; i < num; i++)
            {
                result[i] = start + step * i;
            }
            return result;
        }

        // Convertir de EField a EField_db
        public static double[] EFieldToEFieldDB(string[][] arr)
        {
            //string[][] arr = CSVReader.ReadCSV(@"C:\ENAC\SswApplication\SswApplication\CodeSource\Source\outputs\E_field.csv");
            string[] res = arr.SelectMany(innerArray => innerArray).ToArray();
            Complex[] e_field = res.Select(ParseStringToComplexNumber).ToArray();
            double[] e_field_db = e_field.Select(z => 20 * Math.Log10(z.Magnitude)).ToArray();
            return e_field_db;
        }

        // passer des string aux nombres complex
        private static Complex ParseStringToComplexNumber(string complexString)
        {
            // Define regex pattern to match the real and imaginary parts
            string pattern = @"\((?<real>[-+]?\d+\.\d+e[-+]?\d+)(?<imaginary>[-+]?\d+\.\d+e[-+]?\d+)j\)";

            // Match the pattern using regex
            Match match = Regex.Match(complexString, pattern);

            if (match.Success)
            {
                // Extract and parse the real and imaginary parts from the regex match
                string realPartStr = match.Groups["real"].Value;
                string imaginaryPartStr = match.Groups["imaginary"].Value;

                double realPart = double.Parse(realPartStr, CultureInfo.InvariantCulture);
                double imaginaryPart = double.Parse(imaginaryPartStr, CultureInfo.InvariantCulture);

                return new Complex(realPart, imaginaryPart);
            }
            else
            {
                throw new FormatException("Invalid complex number format: " + complexString);
            }
        }

        // tableau de double en string 
        public static string DoubleArrayToString(double[] arr)
        {
            string str = string.Empty;
            foreach (double val in arr)
            {
                str += val.ToString(CultureInfo.InvariantCulture) + "\n";
            }
            return str;
        }


    }
}
