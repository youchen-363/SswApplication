﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Measurement;

namespace SswApplication.CSharp.Propagation
{
	internal class DataPropa
	{

		// Propagation

		/// <summary>
		/// Extraire les donnees dans le fichier csv du input source
		/// </summary>
		/// <returns></returns>
		public static ConfigPropa ExtractInputCSVPropa()
		{
			string[][] data = FileFunctions.ReadCSV("CodeSource/propagation/inputs/", "configuration.csv");
			ConfigPropa config = new();
			foreach (string[] dataArray in data)
			{
				AssignValues(config, dataArray);
			}
			return config;
		}

		/// <summary>
		/// Choisir l'attribut correspondant en fonction du nom et affecter les valeurs dans cet attribut
		/// </summary>
		/// <param name="config">configuration a mettre a jour</param>
		/// <param name="data">donnees necessaires</param>
		/// <exception cref="Exception"></exception>
		private static void AssignValues(ConfigPropa config, string[] data)
		{
			switch (data[0])
			{
				case "Property":
					break;
				case "method":
					ValuesExceptions.CheckMethod(data[1]);
					config.Method.UpdateMeasurement(data);
					break;
				case "N_x":
					ValuesExceptions.CheckNegativeNumber(double.Parse(data[1], CultureInfo.InvariantCulture));
					config.N_x.UpdateMeasurement(data);
					break;
				case "N_z":
					ValuesExceptions.CheckNz(double.Parse(data[1], CultureInfo.InvariantCulture), config.WaveletLevel.Value);
					config.N_z.UpdateMeasurement(data);
					break;
				case "x_step":
					config.X_step.UpdateMeasurement(data);
					break;
				case "z_step":
					config.Z_step.UpdateMeasurement(data);
					break;
				case "frequency":
					config.Frequency.UpdateMeasurement(data);
					break;
				case "polarisation":
					ValuesExceptions.CheckPolarisation(data[1]);
					config.Polarisation.UpdateMeasurement(data);
					break;
				case "Max compression error":
					config.MaxCompressionError.UpdateMeasurement(data);
					break;
				case "wavelet level":
					ValuesExceptions.CheckNz(config.N_z.Value, double.Parse(data[1], CultureInfo.InvariantCulture));
					config.WaveletLevel.UpdateMeasurement(data);
					break;
				case "wavelet family":
					ValuesExceptions.CheckWaveletFamily(data[1]);
					config.WaveletFamily.UpdateMeasurement(data);
					break;
				case "apodisation window":
					ValuesExceptions.CheckApodisationType(data[1]);
					config.ApodisationWindow.UpdateMeasurement(data);
					break;
				case "apodisation size":
					ValuesExceptions.CheckApodisationSize(double.Parse(data[1], CultureInfo.InvariantCulture));
					config.ApodisationSize.UpdateMeasurement(data);
					break;
				case "image size":
					ValuesExceptions.CheckImageLayer(double.Parse(data[1], CultureInfo.InvariantCulture));
					config.ImageSize.UpdateMeasurement(data);
					break;
				case "ground":
					ValuesExceptions.CheckGroundType(data[1]);
					config.Ground.UpdateMeasurement(data);
					break;
				case "epsr":
					config.Epsr.UpdateMeasurement(data);
					break;
				case "sigma":
					config.Sigma.UpdateMeasurement(data);
					break;
				case "atmosphere":
					ValuesExceptions.CheckAtmosphereType(data[1]);
					config.Atmosphere.UpdateMeasurement(data);
					break;
				case "turbulence":
					ValuesExceptions.CheckTurbulence(data[1]);
					config.Turbulence.UpdateMeasurement(data);
					break;
				case "Cn2":
					config.Cn2.UpdateMeasurement(data);
					break;
				case "L0":
					config.L0.UpdateMeasurement(data);
					break;
				case "c0":
					config.C0.UpdateMeasurement(data);
					break;
				case "delta":
					config.Delta.UpdateMeasurement(data);
					break;
				case "zb":
					config.Zb.UpdateMeasurement(data);
					break;
				case "c2":
					config.C2.UpdateMeasurement(data);
					break;
				case "zt":
					config.Zt.UpdateMeasurement(data);
					break;
				case "atm filename":
					config.AtmFilename.UpdateMeasurement(data);
					break;
				case "dynamic":
					config.Dynamic.UpdateMeasurement(data);
					break;
				case "py_or_cy":
					ValuesExceptions.CheckPyOrCy(data[1]);
					config.PyOrCy.UpdateMeasurement(data);
					break;
				default:
					throw new Exception($"Output file of the relief generation is not valid. Input {data[0]} not valid.");
			}
		}

		/// <summary>
		/// Execute le fichier executable de propagation
		/// </summary>
		public static string ExecutePropagation()
		{
			return FileFunctions.ExecuteExe("CodeSource/propagation/", "main_propagation.exe");
		}

		/// <summary>
		/// Ecrire les donnees dans le fichier csv du input propagation 
		/// </summary>
		/// <param name="config">Une instance de la configuration propagation contenant des valeurs</param>
		public static void WriteInputPropagation(ConfigPropa config)
		{
			string[][] fields =
				[
				["Property", "Value", "Unit"],
				MeasString.CreateField(config.Method),
				MeasNumber.CreateField(config.N_x),
				MeasNumber.CreateField(config.N_z),
				MeasNumber.CreateField(config.X_step),
				MeasNumber.CreateField(config.Z_step),
				MeasNumber.CreateField(config.Frequency),
				MeasString.CreateField(config.Polarisation),
				MeasNumber.CreateField(config.MaxCompressionError),
				MeasNumber.CreateField(config.WaveletLevel),
				MeasString.CreateField(config.WaveletFamily),
				MeasString.CreateField(config.ApodisationWindow),
				MeasNumber.CreateField(config.ApodisationSize),
				MeasNumber.CreateField(config.ImageSize),
				MeasString.CreateField(config.Ground),
				MeasNumber.CreateField(config.Epsr),
				MeasNumber.CreateField(config.Sigma),
				MeasString.CreateField(config.Atmosphere),
				MeasString.CreateField(config.Turbulence),
				MeasNumber.CreateField(config.Cn2),
				MeasNumber.CreateField(config.L0),
				MeasNumber.CreateField(config.C0),
				MeasNumber.CreateField(config.Delta),
				MeasNumber.CreateField(config.Zb),
				MeasNumber.CreateField(config.C2),
				MeasNumber.CreateField(config.Zt),
				MeasString.CreateField(config.AtmFilename),
				MeasNumber.CreateField(config.Dynamic),
				MeasString.CreateField(config.PyOrCy)
				];
			FileFunctions.WriteCSV("CodeSource/propagation/inputs/", "configuration.csv", fields);
		}

	private static List<double> AxesValues(int nbPts, double step)
		{
			List<double> res = [];
			for (int i = 0; i < nbPts; i++)
			{
				res.Add(i * step);
			}
			return res;
		}

		public static List<double> XValues(ConfigPropa config)
		{
			return AxesValues((int)config.N_x.Value, config.X_step.Value / 1000);
		}

		public static List<double> ZValues(ConfigPropa config)
		{
			return AxesValues((int)config.N_z.Value, config.Z_step.Value);
		}

		public static double VMax(double[][] data)
		{
			double vmax = data[0].Max();
			foreach (double[] dataArray in data)
			{
				double currentMax = dataArray.Max();
				if (currentMax > vmax)
				{
					vmax = currentMax;
				}
			}
			return vmax;
		}

		// config, vmax, vmin, z_vect, data, 
		public static double VMax(double[] data)
		{
			return data.Max();
		}

		public static double VMin(ConfigPropa config, double max)
		{
			return max - config.Dynamic.Value;
		}

		public static double[][] E_Total_Final()
		{
			Complex[][] data = E_Total_Data();
			int length = data[0].Length;
			double[][] finalData = new double[length][];

			for (int i = 0; i < length; i++)
			{
				finalData[i] = new double[data.Length];
				for (int j = 0; j < data.Length; j++)
				{
					finalData[i][j] = 20 * Math.Log10(data[j][i].Magnitude);
				}
			}
			return finalData;
		}

		public static string SerializeToJson(List<double> xValues, List<double> zValues, double[][] yValues, double vmax, double vmin)
		{
			var data = new
			{
				v_max = vmax,
				v_min = vmin,
				x_values = xValues,
				y_values = zValues,
				z_values = yValues
			};
			return JsonConvert.SerializeObject(data, Formatting.Indented);
		}

		// do for E (dB/v) first
		public static Complex[][] E_Total_Data()
		{
			string[][] data = FileFunctions.ReadCSV("CodeSource/propagation/outputs/", "E_total.csv");
			Complex[][] res = new Complex[data.Length][];
			for (int i = 0; i < data.Length; i++)
			{
				res[i] = data[i].Select(CommonFns.ParseStringToComplexNumber).ToArray();
			}
			return res;
		}

		// for z_vect = np.linspace(0, ConfigSource.z_step*ConfigSource.n_z, num=ConfigSource.n_z, endpoint=False)
		/// <summary>
		/// Une methode pour remplacer numpy.linspace en python
		/// </summary>
		/// <param name="config">configuration contenant des valeurs necessaires </param>
		/// <param name="endpoint">point d'arret</param>
		/// <returns></returns>
		public static double[] GenerateValues(ConfigPropa config, bool endpoint)
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


		public static double[] LastColumnData(double[][] finalData)
		{
			double[] res = new double[finalData.Length];
			for (int i = 0; i < finalData.Length; i++)
			{
				int lastIdx = finalData[i].Length - 1;
				res[i] = finalData[i][lastIdx];
			}
			return res;
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
		public static string SerializeToJson(double vmin, double vmax, double[] eTotalDb, double[] zvect, ConfigPropa conf)
		{

			var data = new
			{
				v_min = vmin,
				v_max = vmax,
				e_total = eTotalDb,
				z_vect = zvect,
				config = conf
			};

			// Serialize the data object to JSON
			return JsonConvert.SerializeObject(data, Formatting.Indented);
		}

	}
}