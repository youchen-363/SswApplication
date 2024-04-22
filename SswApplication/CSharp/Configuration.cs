using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CsvHelper;

namespace SswApplication.CSharp
{
	internal class Configuration
	{
		//output 
		private Measurement _freq;
		private double _n_z;
		private Measurement _z_step;
		private Measurement _x_s;
		private Measurement _z_s;

		private Measurement _p_tx;
		private Measurement _g_tx;
		private Measurement _w0;
		private string _type;

		public Configuration() { 
			_freq = new Measurement(0, "MHz");
			_n_z = 0;
			_z_step = new Measurement(0, "m");
			_x_s = new Measurement(0, "m");
			_z_s = new Measurement(0, "m");

			_p_tx = new Measurement(0, "W");
			_g_tx = new Measurement(0, "dBi");
			_w0 = new Measurement(0, "m");
			_type = "CSP";
		}

		public Measurement frequency { get => _freq; set => _freq = value;}
		public double N_z { get => _n_z; set => _n_z = value; }
		public Measurement z_step { get => _z_step; set => _z_step = value; }
		public Measurement x_s { get => _x_s; set => _x_s = value; }
		public Measurement z_s { get => _z_s; set => _z_s = value; }
		public Measurement P_Tx { get => _p_tx; set => _p_tx = value; }
		public Measurement G_Tx { get => _g_tx; set => _g_tx = value; }
		public Measurement W0 { get => _w0; set => _w0 = value; }
		public string type { get => _type; set => _type = value; }

		public static Configuration ExtractOutputDataCSV(string path)
		{
			string[][] data = CSVReader.ReadCSV(path);
			Configuration config = new()
			{
				frequency = new Measurement(double.Parse(data[0][1], CultureInfo.InvariantCulture), data[0][2]),
				N_z = Double.Parse(data[1][1], CultureInfo.InvariantCulture),
				z_step = new Measurement(Double.Parse(data[2][1], CultureInfo.InvariantCulture), data[2][2]),
				x_s = new Measurement(Double.Parse(data[3][1], CultureInfo.InvariantCulture), data[3][2]),
				z_s = new Measurement(Double.Parse(data[4][1], CultureInfo.InvariantCulture), data[4][2])
			};
			return config;
		}

		public static Configuration ExtractInputDataCSV(string path)
		{
			string[][] data = CSVReader.ReadCSV(path);
			Configuration config = new()
			{
				N_z = Double.Parse(data[1][1], CultureInfo.InvariantCulture),
				z_step = new Measurement(Double.Parse(data[2][1], CultureInfo.InvariantCulture), data[2][2]),
				x_s = new Measurement(Double.Parse(data[3][1], CultureInfo.InvariantCulture), data[3][2]),
				frequency = new Measurement(Double.Parse(data[4][1], CultureInfo.InvariantCulture), data[4][2]),
				P_Tx = new Measurement(Double.Parse(data[5][1], CultureInfo.InvariantCulture), data[5][2]),
				G_Tx = new Measurement(Double.Parse(data[6][1], CultureInfo.InvariantCulture), data[6][2]),
				type = data[7][1],
				z_s = new Measurement(Double.Parse(data[8][1], CultureInfo.InvariantCulture), data[8][2]),
				W0 = new Measurement(Double.Parse(data[9][1], CultureInfo.InvariantCulture), data[9][2])
			};
			return config;
		}
		/*
		public static void WriteInputDataCSV(string path, Configuration config)
		{
			try
			{
				using (var writer = new StreamWriter(path))
				using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
				{
					csvWriter.WriteField("Property");
					csvWriter.WriteField("Value");
					csvWriter.WriteField("Unit");
					csvWriter.NextRecord();
					
					for (int i=0; i<9; i++) {
						csvWriter.WriteField("Property");
						csvWriter.WriteField("Value");
						csvWriter.WriteField("Unit");
						csvWriter.NextRecord();
					}

					csvWriter.WriteField("frequency");
					csvWriter.WriteField("1000.0");
					csvWriter.WriteField("MHz");
					csvWriter.NextRecord();

					csvWriter.WriteField("n_z");
					csvWriter.WriteField("10");
					csvWriter.WriteField("m");
					csvWriter.NextRecord();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
		*/
		public string ToJSON()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		internal class Measurement(double value, string unit)
		{
			public double Value { get; set; } = value;
			public string Unit { get; set; } = unit;

		}

	}
}
