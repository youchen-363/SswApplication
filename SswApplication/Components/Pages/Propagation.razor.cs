using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NumSharp;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Propagation;
using SswApplication.CSharp.Source;
using SswApplication.CSharp.Units;

namespace SswApplication.Components.Pages
{
	public partial class Propagation
	{
		// les constants pour le nombre de variables de désactivation 
		private const int nbAttrAtm = 6;
		private const int nbAttrGround = 3;
		private const int nbAttrTurbulence = 2;

		private readonly ConfigPropa config = DataPropa.ExtractInputCSVPropa();
		private readonly ConfigSrc configS = DataSrc.ExtractInputCSVSource();

		// les tableaux pour les désactivation des inputs html (atmosphere, ground, turbulence)
		private readonly bool[] disabledAttrGround = new bool[nbAttrGround], disabledTurbulence = new bool[nbAttrTurbulence];

		// les variables qui ne sont pas dans la configuration mais qu'on a besoin
		private double lambda, imageSize, z_max, x_max, width, apoSize;
		private int imageSizePts;
		private string output = "E (dBV/m)", res = string.Empty;
		private MarkupString resultat = new();
		private string test1 = string.Empty;
		private bool plotted = false;

		/// <summary>
		/// Initialiser les variables qui sont nécessaires
		/// </summary>
		protected override void OnInitialized()
		{
			/* 
			 * Étant donné que j'ai séparé les pages, pour exécuter la simulation, nous avons besoin des données sources
			 * J'ai donc créé une nouvelle configuration pour les données sources
			 * Pas d'inquiétude concernant la mémoire, car C# gère automatiquement la gestion de la mémoire pour nous
			 */
			imageSize = config.ImageSize.Value * 100;
			imageSizePts = ImageSizePts();
			SetGroundAttr();
			SetTurbulenceAttr();
			lambda = Physics.Lambda(config.Frequency.Value);
			width = configS.W0.Value / lambda;
			x_max = config.X_step.Value * config.N_x.Value * 1e-3;
			z_max = config.Z_step.Value * config.N_z.Value;
			apoSize = config.ApodisationSize.Value * 100;
		}

		/// <summary>
		/// 0 => Cn2
		/// 1 => L0
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		private void SetTurbulenceAttr()
		{
			disabledTurbulence[0] = config.Turbulence.Value switch
			{
				"N" => disabledTurbulence[1] = true,
				"Y" => disabledTurbulence[1] = false,
				_ => throw new ArgumentException("Turbulence invalide"),
			};
		}

		/// <summary>
		/// 0 => Image size
		/// 1 => epsr
		/// 2 => sigma
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		private void SetGroundAttr()
		{
			switch (config.Ground.Value)
			{
				case "None":
					disabledAttrGround[0] = disabledAttrGround[1] = disabledAttrGround[2] = true;
					break;
				case "PEC":
					disabledAttrGround[0] = false;
					disabledAttrGround[1] = disabledAttrGround[2] = true;
					break;
				case "Dielectric":
					disabledAttrGround[0] = disabledAttrGround[1] = disabledAttrGround[2] = false;
					break;
				default:
					throw new ArgumentException("Ground invalide");
			}
		}
		
		/// <summary>
		/// Run simulation 
		/// </summary>
		private async Task LoadData()
		{
			DataPropa.WriteInputPropagation(config);
			res = DataPropa.ExecutePropagation();
			resultat = (MarkupString)res.Replace("\n", "<br>");
			double[][] finalData = DataPropa.E_Total_Final();
			double[] lastColumn = DataPropa.LastColumnData(finalData);
			/*
			string[] teststr = new string[lastColumn.Length];
			for(int i=0;i<lastColumn.Length;i++)
			{
				teststr[i] = lastColumn[i].ToString(CultureInfo.InvariantCulture);
			}
			string[][] testwrite = [teststr];
            FileFunctions.WriteCSV("CodeSource/propagation", "finaldata2.csv", testwrite);
			*/
            
			List<double> xVals = DataPropa.XValues(config);
			List<double> zVals = DataPropa.ZValues(config);
			double vMaxTotal = DataPropa.VMax(finalData);
			double vMinTotal = DataPropa.VMin(config, vMaxTotal);
			string dataTest = DataPropa.SerializeToJson(xVals, zVals, finalData, vMaxTotal, vMinTotal);
			await JSRuntime.InvokeVoidAsync("drawTest", dataTest, plotted);

			double[] z_vect = DataPropa.GenerateValues(config, false);
			double vMaxLast = DataPropa.VMax(lastColumn);
			double vMinLast = DataPropa.VMin(config, vMaxLast);
			string dataFinal = DataPropa.SerializeToJson(vMinLast, vMaxLast, lastColumn, z_vect, config);
			await JSRuntime.InvokeVoidAsync("drawFinal", dataFinal, plotted);

			plotted = true;

			// apres, tracer le heatmap 
			//await JSRuntime.InvokeVoidAsync("drawGraphPropagation");
		}

		/// <summary>
		/// Une methode pour calculer <Imagesize> de la propagation.
		/// Pts est points (Unit de Image size)
		/// La formule est extrait du code python du projet ssw-2d
		/// </summary>
		/// <returns>image points en integer</returns>
		private int ImageSizePts()
		{
			double wv_L = config.WaveletLevel.Value;
			double n_z = config.N_z.Value;
			double image_layer = config.ImageSize.Value;
			int n_im = (int) Math.Round(n_z * image_layer);
			double remain_im = n_im % Math.Pow(2, wv_L);
			if (remain_im != 0)
			{
				n_im += (int)(Math.Pow(2, wv_L) - remain_im);
            }
			return n_im;
		}

		//Listeners

		private void Method()
		{
			ValuesExceptions.CheckMethod(config.Method.Value);
			Listeners.UpdatePropagation(config.Method.Property, config.Method.Value);		
		}

		private void Language()
		{
			ValuesExceptions.CheckPyOrCy(config.PyOrCy.Value);
			Listeners.UpdatePropagation(config.PyOrCy.Property, config.PyOrCy.Value);
		}
		
		private void GroundType()
		{
			ValuesExceptions.CheckGroundType(config.Ground.Value);
			SetGroundAttr(); 
			Listeners.UpdatePropagation(config.Ground.Property, config.Ground.Value);		
		}

		private void ImageSize()
		{
			ValuesExceptions.CheckImageLayer(config.ImageSize.Value);
			imageSizePts = ImageSizePts();
			Listeners.UpdatePropagation(config.ImageSize.Property, imageSize/100);
		}
			
		private void Epsr()
		{
			Listeners.UpdatePropagation(config.Epsr.Property, config.Epsr.Value);
		}

		private void Sigma()
		{
			Listeners.UpdatePropagation(config.Sigma.Property, config.Sigma.Value);
		}

		private void AtmosphereType()
		{
			ValuesExceptions.CheckAtmosphereType(config.Atmosphere.Value);
			Listeners.UpdatePropagation(config.Atmosphere.Property, config.Atmosphere.Value);
		}

		private void C0()
		{
			Listeners.UpdatePropagation(config.C0.Property, config.C0.Value);
		}

		private void Turbulence()
		{
			ValuesExceptions.CheckTurbulence(config.Turbulence.Value);
			SetTurbulenceAttr();
			Listeners.UpdatePropagation(config.Turbulence.Property, config.Turbulence.Value);
		}

		private void Cn2()
		{
			Listeners.UpdatePropagation(config.Cn2.Property, config.Cn2.Value);
		}

		private void L0()
		{
			Listeners.UpdatePropagation(config.L0.Property, config.L0.Value);
		}

		private void Frequency()
		{
			ValuesExceptions.CheckFrequency(config.Frequency.Value);
			lambda = Physics.Lambda(config.Frequency.Value);
			Listeners.UpdateSource(configS.W0.Property, width * lambda); 
			UpdateFrequency();
		}

		private void Lambda()
		{
			config.Frequency.Value = Physics.Frequency(lambda);	
			ValuesExceptions.CheckFrequency(config.Frequency.Value);
			UpdateFrequency();
		}

		private void UpdateFrequency()
		{
			Listeners.UpdateSource(config.Frequency.Property, config.Frequency.Value);
			Listeners.UpdatePropagation(config.Frequency.Property, config.Frequency.Value);
		}

		private void Polarisation()
		{
			ValuesExceptions.CheckPolarisation(config.Polarisation.Value);
			Listeners.UpdatePropagation(config.Polarisation.Property, config.Polarisation.Value);
		}

		private void Xmax()
		{
			ValuesExceptions.CheckNegativeNumber(config.X_step.Value);
			config.N_x.Value = (int) Math.Round(x_max * 1e3 / config.X_step.Value);
			UpdateNx();
		}

		private void DeltaX()
		{
			ValuesExceptions.CheckNegativeNumber(config.X_step.Value);
			config.N_x.Value = (int) Math.Round(x_max * 1e3 / config.X_step.Value);
			UpdateNxXStep();
		}

		private void Nx()
		{
			ValuesExceptions.CheckNegativeNumber(config.N_x.Value);
			config.X_step.Value = x_max * 1e3 / config.N_x.Value;
			UpdateNxXStep();
		}

		private void UpdateNxXStep()
		{
			UpdateNx();
			UpdateXStep();
		}

		private void UpdateNx()
		{
			Listeners.UpdatePropagation(config.N_x.Property, config.N_x.Value);
			Listeners.UpdateRelief(config.N_x.Property, config.N_x.Value);
		}

		private void UpdateXStep()
		{
			Listeners.UpdatePropagation(config.X_step.Property, config.X_step.Value);
			Listeners.UpdateRelief(config.X_step.Property, config.X_step.Value);
		}

		private void Nz()
		{
			ValuesExceptions.CheckNz(config.N_z.Value, config.WaveletLevel.Value);
			config.Z_step.Value = z_max / config.N_z.Value;
			UpdateNzZstep();
		}

		private void DeltaZ()
		{
			ValuesExceptions.CheckNz(config.N_z.Value, config.WaveletLevel.Value);
			config.N_z.Value = (int) Math.Round(z_max/config.Z_step.Value);
			UpdateNzZstep();
		}

		private void ZMax()
		{
			config.N_z.Value = (int) Math.Round(z_max / config.Z_step.Value);
            UpdateNz();			
		}

		private void UpdateNzZstep()
		{
			UpdateNz();
			UpdateZStep();	
		}

		private void UpdateNz()
		{
			Listeners.UpdateSource(config.N_z.Property, config.N_z.Value);
			Listeners.UpdatePropagation(config.N_z.Property, config.N_z.Value);
		}

		private void UpdateZStep()
		{
			Listeners.UpdateSource(config.Z_step.Property, config.Z_step.Value);
			Listeners.UpdatePropagation(config.Z_step.Property, config.Z_step.Value);
		}

		private void ApodisationType()
		{
			Listeners.UpdatePropagation(config.ApodisationWindow.Property, config.ApodisationWindow.Value);
		}

		private void Apodisation()
		{
			config.ApodisationSize.Value = apoSize / 100;
            ValuesExceptions.CheckApodisationSize(config.ApodisationSize.Value);
            Listeners.UpdatePropagation(config.ApodisationSize.Property, config.ApodisationSize.Value);
		}

		private void WaveletFamily()
		{
			ValuesExceptions.CheckWaveletFamily(config.WaveletFamily.Value);
			Listeners.UpdatePropagation(config.WaveletFamily.Property, config.WaveletFamily.Value);
		}

		private void WaveletLevel()
		{
			ValuesExceptions.CheckNz(config.N_z.Value, config.WaveletLevel.Value);
			imageSizePts = ImageSizePts();
			Listeners.UpdatePropagation(config.WaveletLevel.Property, config.WaveletLevel.Value);
		}

		private void Compression()
		{
			Listeners.UpdatePropagation(config.MaxCompressionError.Property, config.MaxCompressionError.Value);
		}

		private void Output()
		{
			ValuesExceptions.CheckOutputType(output);
		}

		private void Dynamic()
		{
			Listeners.UpdatePropagation(config.Dynamic.Property, config.Dynamic.Value);
		}

	}
}
