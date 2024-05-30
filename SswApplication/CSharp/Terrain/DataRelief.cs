using System.Globalization;
using Newtonsoft.Json;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Measurement;

namespace SswApplication.CSharp.Terrain
{
    internal class DataRelief
    {
        public static List<double> X_relief(int stop, double deltax)
        {
            List<double> res = [];
            for (int i = 0; i<stop; i++) {
                res.Add(i*deltax/1000);
            }
            return res;
        }

        public static List<double> Z_relief()
        {
            List<double> res = [];
            string[][] data = FileFunctions.ReadCSV("CodeSource/terrain/outputs/", "z_relief.csv");
            foreach (string[] dataArray in data)
            {
                res.Add(Double.Parse(dataArray[0], CultureInfo.InvariantCulture));
            }
            return res;
        }

		public static string SerializeToJSON(List<double> x, List<double> z, double delta, double xmax, double zmax, string graphId)
		{
			var data = new 
			{
				xVals = x,
				zVals = z,
				xMax = xmax,
				deltax = delta,
				zMax = zmax,
				id = graphId
			};
			return JsonConvert.SerializeObject(data, Formatting.Indented);
		}

		public static List<double> Z_relief_Test()
        {
            List<double> res = [];
            //string[][] data = FileFunctions.ReadCSV("CodeSource/terrain/outputs/", "z_relief.csv");
			string[][] data = FileFunctions.ReadCSV("CodeSource/terrain/outputs/", "relief_in.csv");
			foreach (string[] dataArray in data)
            {
                res.Add(Double.Parse(dataArray[0], CultureInfo.InvariantCulture));
            }
            return res;
        }

		public static Dictionary<double, double> XZValues(List<double> x, List<double> z)
		{
			Dictionary<double, double> xyDict = [];
			for (int i = 0; i < x.Count; i++)
			{
				xyDict[x[i]] = z[i];
			}
			return xyDict;
		}

		public static void WriteInputCSVTerrain(ConfigRelief config)
		{
			string[][] fields =
				[
				["Property","Value","Unit"],
				MeasNumber.CreateField(config.N_x),
				MeasNumber.CreateField(config.X_step),
				MeasString.CreateField(config.Type),
				MeasNumber.CreateField(config.Z_max_relief),
				MeasNumber.CreateField(config.Iterations),
				MeasNumber.CreateField(config.Width),
				MeasNumber.CreateField(config.Center)
				];
			FileFunctions.WriteCSV("CodeSource/terrain/inputs/", "conf_terrain.csv", fields);
		}

		public static void WriteInputCSVTerrain(Dictionary<double, double> values)
		{
			string[][] fields = new string[values.Count][];
			int i = 0;
			foreach (KeyValuePair<double, double> xy in values)
			{
				fields[i] = [xy.Key.ToString(CultureInfo.InvariantCulture), xy.Value.ToString(CultureInfo.InvariantCulture)];
				i++;
			}
			FileFunctions.WriteCSV("CodeSource/terrain/inputs/", "relief_in.csv", fields);
		}

		public static void WriteColumnsTerrain(List<double> x, List<double> y)
		{
			string[][] array = x.Select((value, index) => new string[] { value.ToString(CultureInfo.InvariantCulture), y[index].ToString(CultureInfo.InvariantCulture)}).ToArray();
			FileFunctions.WriteCSV("CodeSource/terrain/inputs/", "relief_in.csv", array);
		}

		public static (List<double >x, List<double> y) ExtractColumnsTerrain()
		{
			string[][] data = FileFunctions.ReadCSV("CodeSource/terrain/inputs", "relief_in.csv"); 
			List<double> x = data.Select(dataArray => Double.Parse(dataArray[0], CultureInfo.InvariantCulture)).ToList();
    		List<double> y = data.Select(dataArray => Double.Parse(dataArray[1], CultureInfo.InvariantCulture)).ToList();
			return (x, y);
		}

		public static ConfigRelief ExtractInputCSVTerrain()
		{
			ConfigRelief config = new();
			string[][] data = FileFunctions.ReadCSV("CodeSource/Terrain/inputs/", "conf_terrain.csv");
			if (data.Length == 0)
			{
				throw new ArgumentException($"Data problem : {data}");
			}
			//config.N_x = new Measurement(data[1][0], Double.Parse(data[1][1], CultureInfo.InvariantCulture));
			foreach (string[] str in data)
			{
				AssignValues(config, str);
			}
			return config;
		}

		public static (string, string) ExecuteRelief()
		{
			return FileFunctions.ExecuteExe("CodeSource/terrain/", "main_terrain.exe");
		}

		private static void AssignValues(ConfigRelief config, string[] data)
		{
			switch (data[0])
			{
				case "Property":
					break;
				case "N_x":
					data[2] = "";
					ValueException.CheckNegativeNumber(double.Parse(data[1], CultureInfo.InvariantCulture));
					config.N_x.UpdateMeasurement(data);
					break;
				case "x_step":
					ValueException.CheckNegativeNumber(double.Parse(data[1], CultureInfo.InvariantCulture));
					config.X_step.UpdateMeasurement(data);
					break;
				case "type":
					config.Type.UpdateMeasurement(data);
					break;
				case "z_max_relief":
					config.Z_max_relief.UpdateMeasurement(data);
					break;
				case "iterations":
					config.Iterations.UpdateMeasurement(data);
					break;
				case "width":
					config.Width.UpdateMeasurement(data);
					break;
				case "center":
					config.Center.UpdateMeasurement(data);
					break;
				default:
					throw new ArgumentException($"Output file of the relief generation is not valid. Input {data[0]} not valid.");
			}
		}
	}
}
