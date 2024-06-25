using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Propagation;
using SswApplication.CSharp.Source;
using SswApplication.CSharp.Units;

namespace SswApplication.Components.Pages
{
	public partial class Propagation
	{
		// les constants pour le nombre de variables de désactivation 
		private const int nbAttrGround = 3, nbAttrTurbulence = 2;
		private readonly ConfigPropa config = DataPropa.ExtractInputCSVPropa();
		private readonly ConfigSrc configS = DataSrc.ExtractInputCSVSource();
		// les tableaux pour les désactivation des inputs html (atmosphere, ground, turbulence)
		private readonly bool[] disabledAttrGround = new bool[nbAttrGround], disabledTurbulence = new bool[nbAttrTurbulence];
		// les variables qui ne sont pas dans la configuration mais qu'on a besoin
		private double lambda, z_max, x_max, width, apoSize, xCol;
		// private int imageSizePts;
		private string output = "E (dBV/m)", res = string.Empty;
		private MarkupString resultat = new();
		private bool plottedGraph = false, plottedFinal = false, disabledFinal, columnDisplay = false;
		private double[][] finalData = [];
		private double[] lastColumn = [];

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
			if (config.Ground.Value != "PEC") 
			{
				throw new ArgumentException("Ground invalide");
			}
			disabledAttrGround[0] = false;
			disabledAttrGround[1] = disabledAttrGround[2] = true;
		}
		
		/// <summary>
		/// Run simulation 
		/// </summary>
		private async Task LoadData()
		{
			await JSRuntime.InvokeVoidAsync("setCursorLoading");
			try
			{
				DataPropa.WriteInputPropagation(config);
				(string output, string error) = DataPropa.ExecutePropagation();
				res = output + error;
				resultat = CommonFns.ReplaceNToBr(res);
				finalData = DataPropa.E_Total_Final();
				xCol = (finalData[0].Length - 1) * config.X_step.Value / 1000;

				if (error == string.Empty)
				{
					columnDisplay = true;
					List<double> xVals = DataPropa.XValues(config);
					List<double> zVals = DataPropa.ZValues(config);
					double vMaxTotal = DataPropa.VMax(finalData);
					double vMinTotal = DataPropa.VMin(config, vMaxTotal);
					string x = DataPropa.SerializeToJson(xVals);
					string y = DataPropa.SerializeToJson(zVals);
					string z = DataPropa.SerializeToJson(finalData);
					await PassDataInChunks(z);
					string vMaxMin = DataPropa.SerializeToJson(vMaxTotal, vMinTotal);

					//string dataTest = DataPropa.SerializeToJson(xVals, zVals, finalData, vMaxTotal, vMinTotal);
					
					await JSRuntime.InvokeVoidAsync("drawGraphPropa", x, y, vMaxMin, plottedGraph);

					plottedGraph = true;
					await DrawFinal();
				}
			}
			finally
			{
				await JSRuntime.InvokeVoidAsync("resetCursor");
			}
		}

		private async Task PassDataInChunks(string data)
        {
            // Define chunk size
            int chunkSize = 1024 * 1024; // For example, 1MB
            
            for (int i = 0; i < data.Length; i += chunkSize)
            {
                string chunk = data.Substring(i, Math.Min(chunkSize, data.Length - i));
                await JSRuntime.InvokeVoidAsync("receiveChunk", chunk);
            }
            
            // Notify JavaScript that all chunks have been sent
            await JSRuntime.InvokeVoidAsync("allChunksSent");
        }

		private async Task DrawFinal()
		{
			await JSRuntime.InvokeVoidAsync("setCursorLoading");
			try
			{
				lastColumn = DataPropa.FinalData(finalData, xCol, config.X_step.Value);
				double[] z_vect = DataPropa.GenerateValues(config, false);
				double vMaxLast = DataPropa.VMax(lastColumn);
				double vMinLast = DataPropa.VMin(config, vMaxLast);
				string dataFinal = DataPropa.SerializeToJson(vMinLast, vMaxLast, lastColumn, z_vect, config);
				await JSRuntime.InvokeVoidAsync("drawFinal", dataFinal, plottedFinal);

				plottedFinal = true;
			}
			finally
			{
				await JSRuntime.InvokeVoidAsync("resetCursor");
			}
		}
		
		//Listeners

		private void CheckFinalColumn()
		{
			disabledFinal = xCol < 0 || xCol >= x_max;
		}
		
		private void GroundType()
		{
			ValueException.CheckGroundType(config.Ground.Value);
			SetGroundAttr(); 
			Listeners.UpdatePropagation(config.Ground.Property, config.Ground.Value);		
		}
		
		private void AtmosphereType()
		{
			ValueException.CheckAtmosphereType(config.Atmosphere.Value);
			Listeners.UpdatePropagation(config.Atmosphere.Property, config.Atmosphere.Value);
		}

		private void C0()
		{
			Listeners.UpdatePropagation(config.C0.Property, config.C0.Value);
		}

		private void Turbulence()
		{
			ValueException.CheckTurbulence(config.Turbulence.Value);
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
			ValueException.CheckFrequency(config.Frequency.Value);
			lambda = Physics.Lambda(config.Frequency.Value);
			Listeners.UpdateSource(configS.W0.Property, width * lambda); 
			UpdateFrequency();
		}

		private void Lambda()
		{
			config.Frequency.Value = Physics.Frequency(lambda);	
			ValueException.CheckFrequency(config.Frequency.Value);
			UpdateFrequency();
		}

		private void UpdateFrequency()
		{
			Listeners.UpdateSource(config.Frequency.Property, config.Frequency.Value);
			Listeners.UpdatePropagation(config.Frequency.Property, config.Frequency.Value);
		}

		private void Polarisation()
		{
			ValueException.CheckPolarisation(config.Polarisation.Value);
			Listeners.UpdatePropagation(config.Polarisation.Property, config.Polarisation.Value);
		}

		private void Xmax()
		{
			ValueException.CheckNegativeNumber(config.X_step.Value);
			config.N_x.Value = (int) Math.Round(x_max * 1e3 / config.X_step.Value);
			UpdateNx();
		}

		private void DeltaX()
		{
			ValueException.CheckNegativeNumber(config.X_step.Value);
			config.N_x.Value = (int) Math.Round(x_max * 1e3 / config.X_step.Value);
			UpdateNxXStep();
		}

		private void Nx()
		{
			ValueException.CheckNegativeNumber(config.N_x.Value);
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
			////ValueException.CheckNz(config.N_z.Value);
			config.Z_step.Value = z_max / config.N_z.Value;
			UpdateNzZstep();
		}

		private void DeltaZ()
		{
			////ValueException.CheckNz(config.N_z.Value);
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
            ValueException.CheckApodisationSize(config.ApodisationSize.Value);
            Listeners.UpdatePropagation(config.ApodisationSize.Property, config.ApodisationSize.Value);
		}
		
		private void Output()
		{
			ValueException.CheckOutputType(output);
		}

		private void Dynamic()
		{
			Listeners.UpdatePropagation(config.Dynamic.Property, config.Dynamic.Value);
		}

	}
}
