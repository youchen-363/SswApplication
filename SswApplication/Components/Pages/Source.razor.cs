using System.Globalization;
using System.Numerics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Source;
using SswApplication.CSharp.Units;

namespace SswApplication.Components.Pages
{
	public partial class Source
	{
		private readonly ConfigSrc config = DataSrc.ExtractInputCSVSource();
		private string data = string.Empty, res = string.Empty;
		private MarkupString resultat = new();
		private Boolean plotted = false;

		// les variables pour les données nécessaires mais pas dans le fichier input
		private double z_max, lambda, width, x_s;

		private string resData = string.Empty;

		/// <summary>
		/// Initialisation des variables nécessaires
		/// </summary>
		protected override void OnInitialized()		
		{ 
			z_max = config.Z_step.Value * config.N_z.Value;
			lambda = Physics.Lambda(config.Frequency.Value);
			width = config.W0.Value / lambda;
			x_s = Math.Abs(config.X_s.Value);
		}

		/// <summary>
		/// Dessiner le graphe de la source 
		/// </summary>
		/// <returns>Une tâche asynchrone représentant l'opération.</returns>
		private async Task DrawChart()
		{
			try
			{
				// Mettre a jour les données dans le fichier CSV input config
				DataSrc.WriteInputCSVSource(config);
				DataSrc.WriteOutputConfigSource(config);
				
				List<Complex> eTotal = DataSrc.ETotal(config);
				DataSrc.WriteETotal(eTotal);
				data = DataSrc.InitialiseData(config);
				await JsRuntime.InvokeVoidAsync("drawSource", data, plotted);
				plotted = true;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error drawing chart: {ex.Message}");
			}
		}

		// Listener

		private void Zs()
		{
			ValueException.CheckZs(config.Z_s.Value, z_max);
			Listeners.UpdateSource(config.Z_s.Property, config.Z_s.Value);
		}

		private void Xs()
		{
			config.X_s.Value = -x_s;
			ValueException.CheckXs(config.X_s.Value);
			Listeners.UpdateSource(config.X_s.Property, config.X_s.Value);
		}

		private void Frequency()
		{
			ValueException.CheckFrequency(config.Frequency.Value);
			lambda = Physics.SpeedOfLight / (config.Frequency.Value * 1e6);
			UpdateFrequency();
			config.W0.Value = width * lambda;
			Listeners.UpdateSource(config.W0.Property, config.W0.Value);
		}

		private void Lambda()
		{
			ValueException.CheckNegativeNumber(lambda);
			config.Frequency.Value = Physics.SpeedOfLight / lambda * 1e-6;
			UpdateFrequency();					
		}

		private void UpdateFrequency()
		{
			Listeners.UpdateSource(config.Frequency.Property, config.Frequency.Value);
			Listeners.UpdatePropagation(config.Frequency.Property, config.Frequency.Value); 
		}

		private void Nz()
		{
			config.Z_step.Value = z_max / config.N_z.Value;
			UpdateNz();
			UpdateZStep();
		}

		private void ZStep()
		{
			ValueException.CheckZStep(config.Z_step.Value);
			config.N_z.Value = (int) Math.Round(z_max/config.Z_step.Value);
			UpdateNz();
			UpdateZStep();
		}

		private void ZMax()
		{
			config.N_z.Value = (int) Math.Round(z_max / config.Z_step.Value);
			UpdateNz();	
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

	}

}
