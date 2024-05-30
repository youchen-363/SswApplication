using System.Globalization;
using System.Numerics;
using Newtonsoft.Json;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Measurement;
using SswApplication.CSharp.Propagation;
using SswApplication.CSharp.Units;

namespace SswApplication.CSharp.Source
{
    internal class DataSrc
    {
        /// <summary>
        /// Initialiser les données nécessaires pour tracer le graphe en format json
        /// </summary>
        /// <returns>données en string format json</returns>
        public static string InitialiseDataSrc()
        {
            // pas passe
            ConfigSrc config = ExtractOutputCSVSource();
			ConfigPropa configP = DataPropa.ExtractInputCSVPropa();
            double[] efield_db = EFieldToEFieldDB();
            double v_max = MaxInArray(efield_db);
            double v_min = VMin(v_max, configP.Dynamic.Value);
            double[] z_vect = GenerateValues(config, false);
            string json = SerializeToJson(v_min, v_max, efield_db, z_vect, config);
            return json;
        }

		public static string InitialiseTestData()
		{
            // pas passe
            ConfigSrc config = ExtractOutputCSVSource();
			ConfigPropa configP = DataPropa.ExtractInputCSVPropa();
			double[] etotal = EFieldToEFieldDB();
			//double[] etotal = eField.Select(x => x.Real + x.Imaginary).ToArray();
            double v_max = MaxInArray(etotal);
            double v_min = VMin(v_max, configP.Dynamic.Value);
            double[] z_vect = GenerateValues(config, false);
            string json = SerializeToJson(v_min, v_max, etotal, z_vect, config);
            return json;
        }

		private static double RData(ConfigSrc config, Func<double> r)
		{
			double firstValue = r();
			double secondValue = Math.Pow(config.X_s.Value, 2);
			return Math.Sqrt(firstValue + secondValue);
		}

		private static double R1Val(ConfigSrc config, double z)
		{
			return Math.Pow(z-config.Z_s.Value, 2);
		}

		private static double R2Val(ConfigSrc config, double z)
		{
			return Math.Pow(z+config.Z_s.Value, 2);
		}

		public static double R1(ConfigSrc config, double z)
		{
			return RData(config, ()=>R1Val(config, z));
		}

		public static double R2(ConfigSrc config, double z)
		{
			return RData(config, ()=>R2Val(config, z));
		}
		
		public static Complex E1(ConfigSrc config, double r)
		{
			Complex j = Complex.ImaginaryOne;
			double twoPi = 2 * Math.PI;
			double lambda = Physics.Lambda(config.Frequency.Value);
			Complex e = 1/r * Complex.Exp(-j * twoPi * r / lambda);
			return e;
		}

		public static Complex E2(ConfigSrc config, double r)
		{
			Complex j = Complex.ImaginaryOne;
			double twoPi = 2 * Math.PI;
			double lambda = Physics.Lambda(config.Frequency.Value);
			Complex e = 1/r * Complex.Exp(-j * ((twoPi * r / lambda) + Math.PI));
			return e;
		}
		

		public static List<Complex> ETotal (ConfigSrc config)
		{
			double zmax = config.ZMax();
			List<Complex> eTotal = [];
			for (double i = 0; i < zmax; i+=config.Z_step.Value)
			{
				Complex e1 = E1(config, R1(config, i));
				Complex e2 = E2(config, R2(config, i));
				eTotal.Add(e1 + e2);
			}
			return eTotal;
		}

		public static void WriteETotal(List<Complex> eTotal)
		{
			string[][] data = eTotal.Select(complex => new string[] { CommonFns.ComplexToString(complex) }).ToArray();
			FileFunctions.WriteCSV("CodeSource/source/outputs", "E_field.csv", data);
		}

		/// <summary>
		/// Passer ces données dans un format json 
		/// </summary>
		/// <param name="vmin"></param>
		/// <param name="vmax"></param>
		/// <param name="efielddb"></param>
		/// <param name="zvect"></param>
		/// <param name="conf"></param>
		/// <returns>chaine de caractères avec tous ces valeurs</returns>
		public static string SerializeToJson(double vmin, double vmax, double[] efielddb, double[] zvect, ConfigSrc conf)
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


		// for z_vect = np.linspace(0, ConfigSource.z_step*ConfigSource.n_z, num=ConfigSource.n_z, endpoint=False)
		/// <summary>
		/// Une methode pour remplacer numpy.linspace en python
		/// </summary>
		/// <param name="config">configuration contenant des valeurs necessaires </param>
		/// <param name="endpoint">point d'arret</param>
		/// <returns></returns>
		public static double[] GenerateValues(ConfigSrc config, bool endpoint)
		{
			double start = 0;
			double stop = config.ZMax();
			int num = (int)config.N_z.Value;

			double[] result = new double[num];
			double step = (stop - start) / (endpoint ? num - 1 : num);

			for (int i = 0; i < num; i++)
			{
				result[i] = start + step * i;
			}
			return result;
		}

		// to find v_max
		/// <summary>
		/// retourner la plus grande valeur dans le tableau arr
		/// utilisé pour trouver vmax 
		/// </summary>
		/// <param name="arr">tableau avec des valeurs</param>
		/// <returns>plus grande valeur dans le tableau</returns>
		public static double MaxInArray(double[] arr)
        {
            return arr.Max(x => x);
        }

		// find v_min
		/// <summary>
		/// retourner la plus petite valeur dans le tableau arr
		/// </summary>
		/// <param name="arr">tableau avec des valeurs</param>
		/// <returns>vmin en type double</returns>
		public static double VMin(double v_max, double dynamic)
        {
            return v_max - dynamic;
        }

		private static Complex[] EField()
		{
			string[][] arr = FileFunctions.ReadCSV("CodeSource/Source/outputs/", "E_field.csv");
            string[] res = arr.SelectMany(innerArray => innerArray).ToArray();
            Complex[] e_field = res.Select(CommonFns.ParseStringToComplexNumber).ToArray();
            return e_field;
        }

        // Convertir de EField a EField_db
        /// <summary>
        /// Convertir de EField à EField_db
        /// </summary>
        /// <param name="arr">tableau de double dimension contenant des valeurs Efield</param>
        /// <returns>tableau d'une dimension contenant les valeurs de nombre complex</returns>
        public static double[] EFieldToEFieldDB()
        {
			Complex[] e_field = EField();
            double[] e_field_db = e_field.Select(z => 20 * Math.Log10(z.Magnitude)).ToArray();
            //double[] e_field_db = e_field.Select(z => z.Magnitude).ToArray();
			return e_field_db;
        }

		/// <summary>
		/// Execute le fichier exe de source (main_source.exe)
		/// </summary>
		public static (string, string) ExecuteSource()
		{
			return FileFunctions.ExecuteExe("CodeSource/source/", "main_source.exe");
		}

		/// <summary>
		/// Extraire les donnees dans le fichier csv du output source dans une instance configuration source
		/// </summary>
		/// <param name="dir">chemin du fichier</param>
		/// <param name="file">nom du fichier</param>
		/// <returns>une instance de configuration source</returns>
		public static ConfigSrc ExtractOutputCSVSource()
		{
			string[][] data = FileFunctions.ReadCSV("CodeSource/Source/outputs/", "configuration.csv");
			ConfigSrc config = new()
			{
				Frequency = new MeasNumber(data[0][0], Double.Parse(data[0][1], CultureInfo.InvariantCulture), data[0][2]),
				N_z = new MeasNumber(data[1][0], Double.Parse(data[1][1], CultureInfo.InvariantCulture)),
				Z_step = new MeasNumber(data[2][0], Double.Parse(data[2][1], CultureInfo.InvariantCulture), data[2][2]),
				X_s = new MeasNumber(data[3][0], Double.Parse(data[3][1], CultureInfo.InvariantCulture), data[3][2]),
				Z_s = new MeasNumber(data[4][0], Double.Parse(data[4][1], CultureInfo.InvariantCulture), data[4][2])
			};
			return config;
		}

		/// <summary>
		/// Extraire les donnees dans le fichier csv du input source dans une instance de configuration source
		/// </summary>
		/// <returns>une instance de configuration source</returns>
		public static ConfigSrc ExtractInputCSVSource()
		{
			string[][] data = FileFunctions.ReadCSV("CodeSource/source/inputs", "configuration.csv");
			ConfigSrc config = new();
			foreach (string[] dataArray in data)
			{
				ChooseProperty(config, dataArray);
			}
			return config;
		}

		/// <summary>
		/// Choisir l'attribut correspondant en fonction du nom et affecter les valeurs dans cet attribut
		/// </summary>
		/// <param name="config"></param>
		/// <param name="data"></param>
		/// <exception cref="ArgumentException"></exception>
		public static void ChooseProperty(ConfigSrc config, string[] data)
		{
			switch (data[0])
			{
				case "Property":
					break;
				case "N_z":
					ValueException.CheckNegativeNumber(double.Parse(data[1], CultureInfo.InvariantCulture));
					config.N_z.UpdateMeasurement(data);
					break;
				case "z_step":
					ValueException.CheckNegativeNumber(double.Parse(data[1], CultureInfo.InvariantCulture));
					config.Z_step.UpdateMeasurement(data);
					break;
				case "x_s":
					// dans csv x_s est positif mais sur interface il doit etre negatif
					double x = Double.Parse(data[1], CultureInfo.InvariantCulture);
					ValueException.CheckXs(x);
					config.X_s.UpdateMeasurement(data);
					break;
				case "frequency":
					config.Frequency.UpdateMeasurement(data);
					break;
				case "P_Tx":
					config.P_Tx.UpdateMeasurement(data);
					break;
				case "G_Tx":
					config.G_Tx.UpdateMeasurement(data);
					break;
				case "type":
					ValueException.CheckTypeSource(data[1]);
					config.Type.UpdateMeasurement(data);
					break;
				case "z_s":
					config.Z_s.UpdateMeasurement(data);
					break;
				case "W0":
					config.W0.UpdateMeasurement(data);
					break;
				default:
					throw new ArgumentException($"Output file of the relief generation is not valid. Input {data[0]} not valid.");
			}
		}

		// configuration.csv source
		/// <summary>
		/// Ecrire les donnees dans le fichier csv du input source 
		/// </summary>
		/// <param name="config">Instance de Configuration source</param>
		public static void WriteInputCSVSource(ConfigSrc config)
		{
			string[][] fields =
			[
				["Property", "Value", "Unit"],
				MeasNumber.CreateField(config.N_z),
				MeasNumber.CreateField(config.Z_step),
				MeasNumber.CreateField(config.X_s),
				MeasNumber.CreateField(config.Frequency),
				MeasNumber.CreateField(config.P_Tx),
				MeasNumber.CreateField(config.G_Tx),
				MeasString.CreateField(config.Type),
				MeasNumber.CreateField(config.Z_s),
				MeasNumber.CreateField(config.W0)
			];
			FileFunctions.WriteCSV("CodeSource/source/inputs/", "configuration.csv", fields);
		}
	}
}
