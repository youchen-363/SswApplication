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
        private MeasurementNumber _n_z;
        private MeasurementNumber _z_step;
        private MeasurementNumber _x_s;
        private MeasurementNumber _freq;
        private MeasurementNumber _p_tx;
        private MeasurementNumber _g_tx;
        private MeasurementString _type;
        private MeasurementNumber _z_s;
        private MeasurementNumber _w0;

        public ConfigurationSource()
        {
            _freq = new MeasurementNumber("frequency", 0, "MHz");
            _n_z = new MeasurementNumber("N_z", 0);
            _z_step = new MeasurementNumber("z_step", 0, "m");
            _x_s = new MeasurementNumber("x_s", 0, "m");
            _z_s = new MeasurementNumber("z_s", 0, "m");

            _p_tx = new MeasurementNumber("P_Tx", 0, "W");
            _g_tx = new MeasurementNumber("G_Tx", 0, "dBi");
            _w0 = new MeasurementNumber("W0", 0, "m");
            _type = new MeasurementString("type", "CSP");
        }
        public MeasurementNumber N_z { get => _n_z; set => _n_z = value; }
        public MeasurementNumber Z_step { get => _z_step; set => _z_step = value; }
        public MeasurementNumber X_s { get => _x_s; set => _x_s = value; }
        public MeasurementNumber Frequency { get => _freq; set => _freq = value; }
        public MeasurementNumber P_Tx { get => _p_tx; set => _p_tx = value; }
        public MeasurementNumber G_Tx { get => _g_tx; set => _g_tx = value; }
        public MeasurementString Type { get => _type; set => _type = value; }
        public MeasurementNumber Z_s { get => _z_s; set => _z_s = value; }
        public MeasurementNumber W0 { get => _w0; set => _w0 = value; }

        // configuration.csv source
        public static ConfigurationSource ExtractOutputCSVSource(string path)
        {
            string[][] data = FileFunctions.ReadCSV(path);
            ConfigurationSource config = new()
            {
                Frequency = new MeasurementNumber(data[0][0], Double.Parse(data[0][1], CultureInfo.InvariantCulture), data[0][2]),
                N_z = new MeasurementNumber(data[1][0], Double.Parse(data[1][1], CultureInfo.InvariantCulture)),
                Z_step = new MeasurementNumber(data[2][0], Double.Parse(data[2][1], CultureInfo.InvariantCulture), data[2][2]),
                X_s = new MeasurementNumber(data[3][0], Double.Parse(data[3][1], CultureInfo.InvariantCulture), data[3][2]),
                Z_s = new MeasurementNumber(data[4][0], Double.Parse(data[4][1], CultureInfo.InvariantCulture), data[4][2])
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
                    config.Type.UpdateMeasurement(data);
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
        public static void WriteInputCSVSource(string dir, string file, ConfigurationSource config)
        {
            string[][] fields =
            [
                ["Property", "Value", "Unit"],
                MeasurementNumber.CreateField(config.N_z),
                MeasurementNumber.CreateField(config.Z_step),
                MeasurementNumber.CreateField(config.X_s),
                MeasurementNumber.CreateField(config.Frequency),
                MeasurementNumber.CreateField(config.P_Tx),
                MeasurementNumber.CreateField(config.G_Tx),
                MeasurementString.CreateField(config.Type),
                MeasurementNumber.CreateField(config.Z_s),
                MeasurementNumber.CreateField(config.W0)
            ];
            FileFunctions.WriteCSV(dir, file, fields);
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
