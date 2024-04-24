using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

/* Unmerged change from project 'SswApplication (net8.0-windows10.0.19041.0)'
Before:
using CsvHelper;
After:
using CsvHelper;
using SswApplication;
using SswApplication.CSharp;
using SswApplication.CSharp.Source;
*/
using CsvHelper;
using System.Diagnostics;

namespace SswApplication.CSharp.Source
{
    internal class ConfigurationSource
    {
        //output 
        private Measurement _n_z;
        private Measurement _z_step;
        private Measurement _x_s;
        private Measurement _freq;
        private Measurement _p_tx;
        private Measurement _g_tx;
        private string _type;
        private Measurement _z_s;
        private Measurement _w0;

        public ConfigurationSource()
        {
            _freq = new Measurement("frequency", 0, "MHz");
            _n_z = new Measurement("N_z", 0);
            _z_step = new Measurement("z_step", 0, "m");
            _x_s = new Measurement("x_s", 0, "m");
            _z_s = new Measurement("z_s", 0, "m");

            _p_tx = new Measurement("P_Tx", 0, "W");
            _g_tx = new Measurement("G_Tx", 0, "dBi");
            _w0 = new Measurement("W0", 0, "m");
            _type = "CSP";
        }
        public Measurement N_z { get => _n_z; set => _n_z = value; }
        public Measurement Z_step { get => _z_step; set => _z_step = value; }
        public Measurement X_s { get => _x_s; set => _x_s = value; }
        public Measurement Frequency { get => _freq; set => _freq = value; }
        public Measurement P_Tx { get => _p_tx; set => _p_tx = value; }
        public Measurement G_Tx { get => _g_tx; set => _g_tx = value; }
        public string Type { get => _type; set => _type = value; }
        public Measurement Z_s { get => _z_s; set => _z_s = value; }
        public Measurement W0 { get => _w0; set => _w0 = value; }

        // configuration.csv source
        public static ConfigurationSource ExtractOutputCSVSource(string path)
        {
            string[][] data = FileFunctions.ReadCSV(path);
            ConfigurationSource config = new()
            {
                Frequency = new Measurement(data[0][0], Double.Parse(data[0][1], CultureInfo.InvariantCulture), data[0][2]),
                N_z = new Measurement(data[1][0], Double.Parse(data[1][1], CultureInfo.InvariantCulture)),
                Z_step = new Measurement(data[2][0], Double.Parse(data[2][1], CultureInfo.InvariantCulture), data[2][2]),
                X_s = new Measurement(data[3][0], Double.Parse(data[3][1], CultureInfo.InvariantCulture), data[3][2]),
                Z_s = new Measurement(data[4][0], Double.Parse(data[4][1], CultureInfo.InvariantCulture), data[4][2])
			};
            return config;
        }

        // faut changer N_z et type 
        public static ConfigurationSource ExtractInputCSVSource(string path)
        {
            string[][] data = FileFunctions.ReadCSV(path);
            ConfigurationSource config = new();
            foreach (string[] dataArray in data)
            {
                ChooseProperty(config, dataArray);
            }
            return config;
        }

        // OK
        public static void ChooseProperty(ConfigurationSource config, string[] data)
        {
            switch (data[0])
            {
                case "Property":
                    break;
                case "N_z":
                    config.N_z.UpdateMeasurement(data);
                    break;
                case "z_step":
                    config.Z_step.UpdateMeasurement(data);
                    break;
                case "x_s":
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
                    config.Type = data[1];
                    break;
                case "z_s":
                    config.Z_s.UpdateMeasurement(data);
                    break;
                case "W0":
                    config.W0.UpdateMeasurement(data);
                    break;
                default:
                    throw new ArgumentException("Property invalide " + data[0]);
            }
        }

        // configuration.csv source 
        public static void WriteInputCSVSource(string path, ConfigurationSource config)
        {
            string[][] fields =
            [
                ["Property", "Value", "Unit"],
                Measurement.CreateField(config.N_z),
                Measurement.CreateField(config.Z_step),
                Measurement.CreateField(config.X_s),
                Measurement.CreateField(config.Frequency),
                Measurement.CreateField(config.P_Tx),
                Measurement.CreateField(config.G_Tx),
                ["type", config.Type, ""],
                Measurement.CreateField(config.Z_s),
                Measurement.CreateField(config.W0)
            ];
            FileFunctions.WriteCSV(path, fields);
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        override
        public string ToString()
        {
            return $"n_z={N_z}, z_step={Z_step}, x_s={X_s}, freq={this.Frequency}, P_tx={P_Tx}, G_Tx={G_Tx}, type={Type}, z_s={Z_s}, W0={W0}"; 
        }
    }
}
