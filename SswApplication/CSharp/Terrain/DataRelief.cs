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

		public static void WriteColumnsTerrain(List<double> x, List<double> y)
		{
			string[][] array = x.Select((value, index) => new string[] { value.ToString(CultureInfo.InvariantCulture), y[index].ToString(CultureInfo.InvariantCulture)}).ToArray();
			FileFunctions.WriteCSV("CodeSource/terrain/inputs/", "relief_in.csv", array);
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
			return FileFunctions.ExecuteExe("CodeSource/terrain/", "./dist/main_terrain/main_terrain.exe");
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
