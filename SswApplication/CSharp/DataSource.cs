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

namespace SswApplication.CSharp
{
	internal class DataSource
	{
		public static string InitialiseData()
		{
			Configuration config = Configuration.ExtractOutputDataCSV(@"C:\ENAC\SswApplication\SswApplication\CodeSource\Source\outputs\configuration.csv");
			double[] efield_db = DataSource.EFieldToEFieldDB(CSVReader.ReadCSV(@"C:\ENAC\SswApplication\SswApplication\CodeSource\Source\outputs\E_field.csv"));
			double v_max = DataSource.MaxInArray(efield_db);
			double v_min = DataSource.MinInArray(v_max);
			double[] z_vect = DataSource.GenerateValues(config, false);

			string json = SerializeToJson(v_min, v_max, efield_db, z_vect, config);

			return json;
			// Specify the output file path
			string outputFilePath = @"C:\ENAC\SswApplication\SswApplication\JavaScript\output.json";

			// Write JSON data to a file
			File.WriteAllText(outputFilePath, json);
		}

		static string SerializeToJson(double vmin, double vmax, double[] efielddb, double[] zvect, Configuration conf)
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
		public static double[] GenerateValues(Configuration config, bool endpoint)
		{
			double start = 0;
			double stop = config.z_step.Value*config.N_z;
			int num = (int) config.N_z;
			
			double[] result = new double[num];
			double step = (stop - start) / (endpoint ? (num - 1) : num);

			for (int i = 0; i < num; i++)
			{
				result[i] = start + step * i;
			}
			return result;
		}

		public static double[] EFieldToEFieldDB(string[][] arr)
		{
			//string[][] arr = CSVReader.ReadCSV(@"C:\ENAC\SswApplication\SswApplication\CodeSource\Source\outputs\E_field.csv");
			string[] res = arr.SelectMany(innerArray => innerArray).ToArray();
			Complex[] e_field = res.Select(ParseStringToComplexNumber).ToArray();
			double[] e_field_db = e_field.Select(z => 20 * Math.Log10(z.Magnitude)).ToArray();
			return e_field_db;
		}

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
