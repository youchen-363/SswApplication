using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SswApplication.CSharp.Propagation
{
	internal class ConfigurationPropagation
	{
		private MeasurementString _method;
		private MeasurementNumber _n_x;
		private MeasurementNumber _n_z;
		private MeasurementNumber _x_step;
		private MeasurementNumber _z_step;
		private MeasurementNumber _frequency;
		private MeasurementString _polarisation;
		private MeasurementNumber _maxCompressionError;
		private MeasurementNumber _waveletLevel;
		private MeasurementString _waveletFamily;
		private MeasurementString _apodisationWindow;
		private MeasurementNumber _apodisationSize;
		private MeasurementNumber _imageSize;
		private MeasurementString _ground;
		private MeasurementNumber _epsr;
		private MeasurementNumber _sigma;
		private MeasurementString _atmosphere;
		private MeasurementString _turbulence;
		private MeasurementNumber _cn2;
		private MeasurementNumber _L0;
		private MeasurementNumber _c0;
		private MeasurementNumber _delta;
		private MeasurementNumber _zb;
		private MeasurementNumber _c2;
		private MeasurementNumber _zt;
		private MeasurementString _atmFilename;
		private MeasurementNumber _dynamic;
		private MeasurementString _pyOrCy;

		public MeasurementString Method { get => _method; set => _method = value; }
		public MeasurementNumber N_x { get => _n_x; set => _n_x = value; }
		public MeasurementNumber N_z { get => _n_z; set => _n_z = value; }
		public MeasurementNumber X_step { get => _x_step; set => _x_step = value; }
		public MeasurementNumber Z_step { get => _z_step; set => _z_step = value; }
		public MeasurementNumber Frequency { get => _frequency; set => _frequency = value; }
		public MeasurementString Polarisation { get => _polarisation; set => _polarisation = value; }
		public MeasurementNumber MaxCompressionError { get => _maxCompressionError; set => _maxCompressionError = value; }
		public MeasurementNumber WaveletLevel { get => _waveletLevel; set => _waveletLevel = value; }
		public MeasurementString WaveletFamily { get => _waveletFamily; set => _waveletFamily = value; }
		public MeasurementString ApodisationWindow { get => _apodisationWindow; set => _apodisationWindow = value; }
		public MeasurementNumber ApodisationSize { get => _apodisationSize; set => _apodisationSize = value; }
		public MeasurementNumber ImageSize { get => _imageSize; set => _imageSize = value; }
		public MeasurementString Ground { get => _ground; set => _ground = value; }
		public MeasurementNumber Epsr { get => _epsr; set => _epsr = value; }
		public MeasurementNumber Sigma { get => _sigma; set => _sigma = value; }
		public MeasurementString Atmosphere { get => _atmosphere; set => _atmosphere = value; }
		public MeasurementString Turbulence { get => _turbulence; set => _turbulence = value; }
		public MeasurementNumber Cn2 { get => _cn2; set => _cn2 = value; }
		public MeasurementNumber L0 { get => _L0; set => _L0 = value; }
		public MeasurementNumber C0 { get => _c0; set => _c0 = value; }
		public MeasurementNumber Delta { get => _delta; set => _delta = value; }
		public MeasurementNumber Zb { get => _zb; set => _zb = value; }
		public MeasurementNumber C2 { get => _c2; set => _c2 = value; }
		public MeasurementNumber Zt { get => _zt; set => _zt = value; }
		public MeasurementString AtmFilename { get => _atmFilename; set => _atmFilename = value; }
		public MeasurementNumber Dynamic { get => _dynamic; set => _dynamic = value; }
		public MeasurementString PyOrCy { get => _pyOrCy; set => _pyOrCy = value; }

		public ConfigurationPropagation()
		{
			_method = new MeasurementString("method", "SSF");
			_n_x = new MeasurementNumber("N_x", 0, "Unnamed: 2");
			_n_z = new MeasurementNumber("N_z", 0);
			_x_step = new MeasurementNumber("x_step", 0, "m");
			_z_step = new MeasurementNumber("z_step", 0, "m");
			_frequency = new MeasurementNumber("frequency", 0, "MHz");
			_polarisation = new MeasurementString("polarisation", "TE"); // Set default value
			_maxCompressionError = new MeasurementNumber("Max compression error", 0, "dB");
			_waveletLevel = new MeasurementNumber("wavelet level", 0);
			_waveletFamily = new MeasurementString("wavelet family", "None");
			_apodisationWindow = new MeasurementString("apodisation window", "None");
			_apodisationSize = new MeasurementNumber("apodisation size", 0, "% of N_z");
			_imageSize = new MeasurementNumber("image size", 0, "% of N_z");
			_ground = new MeasurementString("ground", "None");
			_epsr = new MeasurementNumber("epsr", 0);
			_sigma = new MeasurementNumber("sigma", 0, "S/m");
			_atmosphere = new MeasurementString("atmosphere", "None");
			_turbulence = new MeasurementString("turbulence", "N", "Y or N"); // Set default value
			_cn2 = new MeasurementNumber("Cn2", 0, "Cn2 exponent");
			_L0 = new MeasurementNumber("L0", 0, "m");
			_c0 = new MeasurementNumber("c0", 0, "M-unit/m");
			_delta = new MeasurementNumber("delta", 0, "m");
			_zb = new MeasurementNumber("zb", 0, "m");
			_c2 = new MeasurementNumber("c2", 0, "M-unit/m");
			_zt = new MeasurementNumber("zt", 0, "m");
			_atmFilename = new MeasurementString("atm filename", "None");
			_dynamic = new MeasurementNumber("dynamic", 0, "dB");
			_pyOrCy = new MeasurementString("py_or_cy", "None", "'Python' or 'Cython'"); // Set default value
		}

        public static ConfigurationPropagation ExtractInputCSVSource(string dir, string file)
		{
			DirectoryFn.ChangeDirectory(dir);
			string[][] data = FileFunctions.ReadCSV(file);
			ConfigurationPropagation config = new();
			foreach (string[] dataArray in data)
			{
				AssignValues(config, dataArray);
			}
			return config;
		}

		private static void AssignValues(ConfigurationPropagation config, string[] data)
		{
			switch (data[0]) 
			{
				case "Property":
					break;
                case "method":
                    config.Method.UpdateMeasurement(data);
					break;
				case "N_x":
                    config.N_x.UpdateMeasurement(data);
					break;
				case "N_z":
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
                    config.Polarisation.UpdateMeasurement(data);
                    break;

                case "Max compression error":
                    config.MaxCompressionError.UpdateMeasurement(data);
                    break;

                case "wavelet level":
                    config.WaveletLevel.UpdateMeasurement(data);
                    break;

                case "wavelet family":
                    config.WaveletFamily.UpdateMeasurement(data);
                    break;

                case "apodisation window":
                    config.ApodisationWindow.UpdateMeasurement(data);
                    break;

                case "apodisation size":
                    config.ApodisationSize.UpdateMeasurement(data);
                    break;

                case "image size":
                    config.ImageSize.UpdateMeasurement(data);
                    break;

                case "ground":
                    config.Ground.UpdateMeasurement(data);
                    break;

                case "epsr":
                    config.Epsr.UpdateMeasurement(data);
                    break;

                case "sigma":
                    config.Sigma.UpdateMeasurement(data);
                    break;

                case "atmosphere":
                    config.Atmosphere.UpdateMeasurement(data);
                    break;

                case "turbulence":
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
                    config.PyOrCy.UpdateMeasurement(data);
                    break;

                default:
					throw new Exception("Propagation invalide");
            }
        }

		public static void WriteInputPropagation(string dir, string file, ConfigurationPropagation config)
		{
			string[][] fields =
				[
				["Property", "Value", "Unit"],
				MeasurementString.CreateField(config.Method),
				MeasurementNumber.CreateField(config.N_x),
				MeasurementNumber.CreateField(config.N_z),
				MeasurementNumber.CreateField(config.X_step),
				MeasurementNumber.CreateField(config.Z_step),
				MeasurementNumber.CreateField(config.Frequency),
				MeasurementString.CreateField(config.Polarisation),
				MeasurementNumber.CreateField(config.MaxCompressionError),
				MeasurementNumber.CreateField(config.WaveletLevel),
				MeasurementString.CreateField(config.WaveletFamily),
				MeasurementString.CreateField(config.ApodisationWindow),
				MeasurementNumber.CreateField(config.ApodisationSize),
				MeasurementNumber.CreateField(config.ImageSize),
				MeasurementString.CreateField(config.Ground),
				MeasurementNumber.CreateField(config.Epsr),
				MeasurementNumber.CreateField(config.Sigma),
				MeasurementString.CreateField(config.Atmosphere),
				MeasurementString.CreateField(config.Turbulence),
				MeasurementNumber.CreateField(config.Cn2),
				MeasurementNumber.CreateField(config.L0),
				MeasurementNumber.CreateField(config.C0),
				MeasurementNumber.CreateField(config.Delta),
				MeasurementNumber.CreateField(config.Zb),
				MeasurementNumber.CreateField(config.C2),
				MeasurementNumber.CreateField(config.Zt),
				MeasurementString.CreateField(config.AtmFilename),
				MeasurementNumber.CreateField(config.Dynamic),
				MeasurementString.CreateField(config.PyOrCy)
				];
			FileFunctions.WriteCSV(dir, file, fields);
		}

    }
}
