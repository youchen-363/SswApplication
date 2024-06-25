using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Source;
using SswApplication.CSharp.Terrain;

namespace SswApplication.Components.Pages
{
    public partial class Terrain
    {
        private readonly ConfigRelief config = DataRelief.ExtractInputCSVTerrain();
        private readonly ConfigSrc configS = DataSrc.ExtractInputCSVSource();
        private List<double> xVals = [], zVals = [];
        private double x_max;
		private bool plottedGraph = false, plottedTest = false;
		private readonly bool[] disabledAttr = new bool[4];
        private int colDel = -1, colAdd = -1;
        private string alertMsg = string.Empty, successMsg = string.Empty, res = string.Empty, btnAddStatus = "d-none", btnDelStatus = "d-none";
        private MarkupString resultat = new();
        private string output = string.Empty, error = string.Empty;


        /// <summary>
        /// Initialiser toutes les variables nécessaires
        /// </summary>
        protected override void OnInitialized()
        {
            // initialise le tableau en fonction du type dans le fichier input relief 
			//SetDisabledValues(config.Type.Value);
            //(xVals, zVals) = DataRelief.ExtractColumnsTerrain();
            x_max = config.X_step.Value * config.N_x.Value * 1e-3;
            xVals.Add(0);
            zVals.Add(0);
            xVals.Add(x_max);
            zVals.Add(0);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await DrawTestGraph();
            }
        }

        private void AddColumnAfter()
        {
            xVals.Insert(colAdd+1, (xVals[colAdd]+xVals[colAdd+1])/2);
            zVals.Insert(colAdd+1, 0);
            alertMsg = string.Empty;
            successMsg = $"Column successfully added!";
        }

        private void RowChosen(int index)
        {
            colAdd = index;
            colDel = index;
        }

        private void RowUnchosen()
        {
            colAdd = -1;
            colDel = -1;
        }
        
        private async Task DeleteColumn()
        {
            if (await Confirm($"Do you want to delete column {colDel+1}?"))
            {
                xVals.RemoveAt(colDel);
                zVals.RemoveAt(colDel);
                //delete = CheckSize();
                alertMsg = string.Empty;
                successMsg = $"Column {colDel} successfully deleted!";
            }
        }

        private void CheckXValue(int idx)
        {
            if (idx > 0 && idx < xVals.Count-1)
            {
                double x0 = xVals[idx-1];
                double x1 = xVals[idx];
                double x2 = xVals[idx+1];
                if (x1 < x0 || x1 > x2)
                {
                    xVals[idx] = (x0 + x2) /2;
                    alertMsg = $"X{idx} value must be between {x0} and {x2}!";
                }
            }
        }

        private async ValueTask<bool> Confirm(string message)
        {
            return await JsRuntime.InvokeAsync<bool>("confirmMessage", message);
        }

        private async Task SaveCSV()
        {
			DataRelief.WriteColumnsTerrain(xVals, zVals);
            // Mise a jour des données dans le fichier input du relief
            DataRelief.WriteInputCSVTerrain(config);
            // Execute main_terrain.exe
            (output, error) = DataRelief.ExecuteRelief();
            alertMsg = string.Empty;
            successMsg = $"Data successfully saved!";
		}

		/// <summary>
		/// Dessine un graphique de ligne représentant le relief.
		/// </summary>
		/// <returns>Une tâche asynchrone représentant l'opération.</returns>
		/// <exception cref="Exception"></exception>
		private async Task DrawLineGraph()
        {
            try
            {
                res = output + '\n' + error;
                resultat = CommonFns.ReplaceNToBr(res);
                if (error == string.Empty)
                {
                    // extraire les données de l'axes x 
                    List<double> xref = DataRelief.X_relief(((int)config.N_x.Value) + 1, config.X_step.Value);
                    // extraire les données de l'axes y qui est dans le fichier output 
                    List<double> zref = DataRelief.Z_relief();
                    
                    string data = DataRelief.SerializeToJSON(xref, zref, config.X_step.Value, x_max, configS.Z_step.Value * configS.N_z.Value, "canvasTerrain");
                    // dessine le graphe en appellant la fonction drawTerrain() avec JSInterop en passant les données nécessaires
                    await JsRuntime.InvokeVoidAsync("drawTerrain", data, plottedGraph);
                    plottedGraph = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error draw line graph of terrain: " + ex);
            }
        }

        private async Task DrawTestGraph()
        {
            string data = DataRelief.SerializeToJSON(xVals, zVals, config.X_step.Value, x_max, configS.Z_step.Value * configS.N_z.Value, "canvasTest");
            await JsRuntime.InvokeVoidAsync("drawTerrain", data, plottedTest);
            plottedTest = true;
        }

        private void XMax()
        {
            ValueException.CheckXStep(config.X_step.Value);
            config.N_x.Value = (int) Math.Round(x_max * 1e3 / config.X_step.Value);
            xVals[^1] = x_max;
            UpdateNx();
        }

        private void XStep()
        {
            ValueException.CheckXStep(config.X_step.Value);
            config.N_x.Value = (int) Math.Round(x_max * 1e3 / config.X_step.Value);
            ValueException.CheckNx(config.N_x.Value);
            UpdateXStep();
            UpdateNx();
        }

        private void Nx()
        {
            ValueException.CheckNegativeNumber(config.N_x.Value);
            config.X_step.Value = x_max * 1e3 / config.N_x.Value;
            ValueException.CheckXStep(config.X_step.Value);
            UpdateXStep();
            UpdateNx();
        }

        private void UpdateNx()
        {
            Listeners.UpdateRelief(config.N_x.Property, config.N_x.Value);
            Listeners.UpdatePropagation(config.N_x.Property, config.N_x.Value);
        }

        private void UpdateXStep()
        {
            Listeners.UpdatePropagation(config.X_step.Property, config.X_step.Value);
            Listeners.UpdateRelief(config.X_step.Property, config.X_step.Value);
        }

    }
}
