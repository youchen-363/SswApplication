using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Measurement;

namespace SswApplication.CSharp.Propagation
{
    internal class ConfigPropa
	{
		private MeasString method;
		private MeasNumber n_x;
		private MeasNumber n_z;
		private MeasNumber x_step;
		private MeasNumber z_step;
		private MeasNumber frequency;
		private MeasString polarisation;
		private MeasNumber maxCompressionError;
		private MeasNumber waveletLevel;
		private MeasString waveletFamily;
		private MeasString apodisationWindow;
		private MeasNumber apodisationSize;
		private MeasNumber imageSize;
		private MeasString ground;
		private MeasNumber epsr;
		private MeasNumber sigma;
		private MeasString atmosphere;
		private MeasString turbulence;
		private MeasNumber cn2;
		private MeasNumber l0;
		private MeasNumber c0;
		private MeasNumber delta;
		private MeasNumber zb;
		private MeasNumber c2;
		private MeasNumber zt;
		private MeasString atmFilename;
		private MeasNumber dynamic;
		private MeasString pyOrCy;

		public MeasString Method
		{
			get => method;
			set
			{
				ValuesExceptions.CheckMethod(value.Value);
				method = value;
			}
		}
		public MeasNumber N_x { 
			get => n_x; 
			set 
			{
				ValuesExceptions.CheckNegativeNumber(value.Value);
				n_x = value;
			}  
		}
		public MeasNumber N_z { 
			get => n_z; 
			set {
				double wavelet = WaveletLevel.Value;
				ValuesExceptions.CheckNz(value.Value, wavelet);
				n_z = value;
			}  
		}
		public MeasNumber X_step { get => x_step; set => x_step = value; }
		public MeasNumber Z_step { get => z_step; set => z_step = value; }
		public MeasNumber Frequency { get => frequency; set => frequency = value; }
		public MeasString Polarisation { 
			get => polarisation; 
			set {
				ValuesExceptions.CheckPolarisation(value.Value);
				polarisation = value; 
			} 
		}
		public MeasNumber MaxCompressionError { get => maxCompressionError; set => maxCompressionError = value; }
		public MeasNumber WaveletLevel { 
			get => waveletLevel; 
			set {
				ValuesExceptions.CheckNz(N_z.Value, value.Value);
				waveletLevel = value;
			}  
		}
		public MeasString WaveletFamily { 
			get => waveletFamily; 
			set {
				ValuesExceptions.CheckWaveletFamily(value.Value);
				waveletFamily = value;
			} 
		}
		public MeasString ApodisationWindow { 
			get => apodisationWindow; 
			set {
				ValuesExceptions.CheckApodisationType(value.Value);
				apodisationWindow = value; 
			} 
		}
		public MeasNumber ApodisationSize { 
			get => apodisationSize; 
			set {
				ValuesExceptions.CheckApodisationSize(value.Value);
				apodisationSize = value;
			}  
		}
		public MeasNumber ImageSize { 
			get => imageSize; 
			set {
				ValuesExceptions.CheckImageLayer(value.Value);
				imageSize = value;
			}  
		}
		public MeasString Ground { 
			get => ground; 
			set {
				ValuesExceptions.CheckGroundType(value.Value);
				ground = value;
			}  
		}
		public MeasNumber Epsr { get => epsr; set => epsr = value; }
		public MeasNumber Sigma { get => sigma; set => sigma = value; }
		public MeasString Atmosphere {
			get => atmosphere; 
			set {
				ValuesExceptions.CheckAtmosphereType(value.Value);
				atmosphere = value;
			}  
		}
		public MeasString Turbulence { 
			get => turbulence; 
			set {
				ValuesExceptions.CheckTurbulence(value.Value);
				turbulence = value;
			}  
		}
		public MeasNumber Cn2 { get => cn2; set => cn2 = value; }
		public MeasNumber L0 { get => l0; set => l0 = value; }
		public MeasNumber C0 { get => c0; set => c0 = value; }
		public MeasNumber Delta { get => delta; set => delta = value; }
		public MeasNumber Zb { get => zb; set => zb = value; }
		public MeasNumber C2 { get => c2; set => c2 = value; }
		public MeasNumber Zt { get => zt; set => zt = value; }
		public MeasString AtmFilename { get => atmFilename; set => atmFilename = value; }
		public MeasNumber Dynamic { get => dynamic; set => dynamic = value; }
		public MeasString PyOrCy { 
			get => pyOrCy; 
			set {
				ValuesExceptions.CheckPyOrCy(value.Value);
				pyOrCy = value;
			}  
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ConfigPropa()
		{
			method = new MeasString("method", "SSF");
			n_x = new MeasNumber("N_x", 0, "Unnamed: 2");
			n_z = new MeasNumber("N_z", 0);
			x_step = new MeasNumber("x_step", 0, "m");
			z_step = new MeasNumber("z_step", 0, "m");
			frequency = new MeasNumber("frequency", 0, "MHz");
			polarisation = new MeasString("polarisation", "TE"); // Set default value
			maxCompressionError = new MeasNumber("Max compression error", 0, "dB");
			waveletLevel = new MeasNumber("wavelet level", 0);
			waveletFamily = new MeasString("wavelet family", "None");
			apodisationWindow = new MeasString("apodisation window", "None");
			apodisationSize = new MeasNumber("apodisation size", 0, "% of N_z");
			imageSize = new MeasNumber("image size", 0, "% of N_z");
			ground = new MeasString("ground", "None");
			epsr = new MeasNumber("epsr", 0);
			sigma = new MeasNumber("sigma", 0, "S/m");
			atmosphere = new MeasString("atmosphere", "None");
			turbulence = new MeasString("turbulence", "N", "Y or N"); // Set default value
			cn2 = new MeasNumber("Cn2", 0, "Cn2 exponent");
			l0 = new MeasNumber("L0", 0, "m");
			c0 = new MeasNumber("c0", 0, "M-unit/m");
			delta = new MeasNumber("delta", 0, "m");
			zb = new MeasNumber("zb", 0, "m");
			c2 = new MeasNumber("c2", 0, "M-unit/m");
			zt = new MeasNumber("zt", 0, "m");
			atmFilename = new MeasString("atm filename", "None");
			dynamic = new MeasNumber("dynamic", 0, "dB");
			pyOrCy = new MeasString("py_or_cy", "None", "'Python' or 'Cython'"); // Set default value
		}

		public double XMax()
		{
			return N_x.Value * X_step.Value / 1000;
        }

		public double ZMax()
		{
			return N_z.Value * Z_step.Value;
        }

    }
}
