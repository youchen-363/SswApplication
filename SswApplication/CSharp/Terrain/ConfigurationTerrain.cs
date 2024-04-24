using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using SswApplication.CSharp.Source;

namespace SswApplication.CSharp.Terrain
{
    internal class ConfigurationTerrain
    {
        private Measurement _n_x;
        private Measurement _x_step;
        private string _type;
        private Measurement _z_max_relief;
        private Measurement _iterations;
        private Measurement _width;
        private Measurement _center;

        public ConfigurationTerrain()
        {
            _n_x = new Measurement("N_x", 0);
            _x_step = new Measurement("x_step", 0, "m");
            _type = "Superposed";
            _z_max_relief = new Measurement("z_max_relief", 0, "m");
            _iterations = new Measurement("iterations", 0, "for superposed terrain");
            _width = new Measurement("width", 0, "pts");
            _center = new Measurement("center", 0, "pts");
        }

        public Measurement N_x { get => _n_x; set => _n_x = value; }
        public Measurement X_step { get => _x_step; set => _x_step = value; }
        public string Type { get => _type; set => _type = value; }
        public Measurement Z_max_relief { get =>  _z_max_relief; set => _z_max_relief = value; }
        public Measurement Iterations { get =>  _iterations; set => _iterations = value;}
        public Measurement Width { get => _width; set => _width = value; }
        public Measurement Center { get => _center; set => _center = value; }

        public static void WriteInputCSVTerrain(string path, ConfigurationTerrain config)
        {
            string[][] fields =
                [
                ["Property","Value","Unit"],
                Measurement.CreateField(config.N_x),
                Measurement.CreateField(config.X_step),
                ["type", config.Type, ""],
                Measurement.CreateField(config.Z_max_relief),
                Measurement.CreateField(config.Iterations),
                Measurement.CreateField(config.Width),
                Measurement.CreateField(config.Center)
                ];
            FileFunctions.WriteCSV(path, fields);
        }

		public static ConfigurationTerrain ExtractInputCSVTerrain (string path)
        {
            ConfigurationTerrain config = new();
            string[][] data = FileFunctions.ReadCSV(path);
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

        private static void AssignValues(ConfigurationTerrain config, string[] data)
        {
            switch (data[0])
            {
                case "Property":
                    break;
                case "N_x":
                    data[2] = "";
                    config.N_x.UpdateMeasurement(data);
                    break;
                case "x_step":
                    config.X_step.UpdateMeasurement(data);
					break;
                case "type":
                    config.Type = data[1];
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
                    throw new NotImplementedException("Property name invalide");
            }
        }

        override 
        public string ToString()
        {
            return $" [ N_x={N_x}, x_step={X_step}, type={Type}, z_max_relief={Z_max_relief}, iterations={Iterations}, width={Width}, center={Center} ]";
        }

    }
}
