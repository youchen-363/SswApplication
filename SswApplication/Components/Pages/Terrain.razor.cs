using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Terrain;


namespace SswApplication.Components.Pages
{
    public partial class Terrain
    {
        private readonly ConfigRelief config = DataRelief.ExtractInputCSVTerrain();
        private List<double> xVals = [], zVals = [];
        private double x_max;
		private bool plottedGraph = false, plottedTest = false, delete = false, save = false, add = false;
		private readonly bool[] disabledAttr = new bool[4];
        private int col;
        private string alertMsg = string.Empty, successMsg = string.Empty, res = string.Empty;
        private MarkupString resultat = new();
        /// <summary>
        /// Initialiser toutes les variables nécessaires
        /// </summary>
        protected override void OnInitialized()
        {
            // initialise le tableau en fonction du type dans le fichier input relief 
			SetDisabledValues(config.Type.Value);
            (xVals, zVals) = DataRelief.ExtractColumnsTerrain();
            SortXAndZ();
            delete = save = CheckSize();
            add = CheckMaxSize();
            x_max = config.X_step.Value * config.N_x.Value * 1e-3;
        }

        private bool CheckSize()
        {
            return !(xVals.Count>0);
        }

        private bool CheckMaxSize()
        {
            return xVals.Count >= config.N_x.Value;
        }

        private void AddColumn()
        {
            xVals.Add(0);
            zVals.Add(0);
            delete = save = CheckSize();
            add = CheckMaxSize();
            alertMsg = string.Empty;
            successMsg = $"Column successfully added!";
        }

        private async Task DeleteColumn()
        {
            if (await Confirm($"Do you want to delete column {col}?"))
            {
                if (col > xVals.Count || col <= 0)
                {
                    successMsg = string.Empty;
                    alertMsg = "Index out of range!";
                }
                else
                {
                    xVals.RemoveAt(col - 1);
                    zVals.RemoveAt(col - 1);
                    delete = save = CheckSize();
                    add = CheckMaxSize();
                    alertMsg = string.Empty;
                    successMsg = $"Column {col} successfully deleted!";
                }
            }
        }

        private async ValueTask<bool> Confirm(string message)
        {
            return await JsRuntime.InvokeAsync<bool>("confirmMessage", message);
        }

        private void SaveCSV()
        {
            DataRelief.WriteColumnsTerrain(xVals, zVals);
            alertMsg = string.Empty;
            successMsg = $"Data successfully saved!";            
        }

        private void SortXAndZ()
        {
            // Pair the xVals and zVals together
            var pairs = xVals.Zip(zVals, (x, z) => (x, z)).ToList();

            // Sort the pairs by the xVals
            pairs.Sort((pair1, pair2) => pair1.x.CompareTo(pair2.x));

            // Separate the pairs back into two lists
            xVals = pairs.Select(pair => pair.x).ToList();
            zVals = pairs.Select(pair => pair.z).ToList();
        }

		/// <summary>
		/// Affecte les valeurs de désactivation pour modifier l'état de grisage des éléments HTML input en fonction du type spécifié 
		/// </summary>
		/// <param name="type"></param>
		/// <exception cref="Exception"></exception>
		private void SetDisabledValues(string type)
        {
            switch (type)
            {
                case "Plane":
                    disabledAttr[0] = disabledAttr[1] = disabledAttr[2] = disabledAttr[3] = true;
                    break;
                case "Triangle":
                    disabledAttr[0] = disabledAttr[2] = disabledAttr[3] = false;
                    disabledAttr[1] = true;
                    break;
                case "Superposed":
                    disabledAttr[0] = disabledAttr[1] = false;
                    disabledAttr[2] = disabledAttr[3] = true;
                    break;
                default:
                    throw new Exception("Type invalide");
            }
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
                // Mise a jour des données dans le fichier input du relief
                DataRelief.WriteInputCSVTerrain(config);
                Dictionary<double, double> xyDict = DataRelief.XZValues(xVals, zVals);
                for (int i = 0; i < xVals.Count; i++)
                {
                    xyDict[xVals[i]] = zVals[i];
                }

                DataRelief.WriteInputCSVTerrain(xyDict);
                // Execute main_terrain.exe
                res = DataRelief.ExecuteRelief();
                resultat = CommonFns.ReplaceNToBr(res);

                // extraire les données de l'axes x 
                List<double> xref = DataRelief.X_relief(((int)config.N_x.Value) + 1);
                // extraire les données de l'axes y qui est dans le fichier output 
                List<double> zref = DataRelief.Z_relief();
                
                string data = DataRelief.SerializeToJSON(xref, zref, config.X_step.Value, "canvasTerrain");
                // dessine le graphe en appellant la fonction drawTerrain() avec JSInterop en passant les données nécessaires
                await JsRuntime.InvokeVoidAsync("drawTerrain", data, plottedGraph);
                plottedGraph = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error draw line graph of terrain: " + ex);
            }
        }

        private async Task DrawTestGraph()
        {
            (List<double> x, List<double>z) = DataGraph();
            string data = DataRelief.SerializeToJSON(x, z, config.X_step.Value, "canvasTest");
            await JsRuntime.InvokeVoidAsync("drawTerrain", data, plottedTest);
            plottedTest = true;
        }

        private (List<double>, List<double>) DataGraph()
        {
            List<double> x = new(xVals);
            List<double> z = new(zVals);
            if (x[0] != 0)
            {
                x.Insert(0, 0);
                z.Insert(0, 0);
            }
            if (x[^1] != config.N_x.Value)
            {
                x.Add(config.N_x.Value);
                z.Add(0);
            }
            return (x,z);
        } 

        private void Type()
        {
            ValuesExceptions.CheckTerrainType(config.Type.Value);
            Listeners.UpdateRelief(config.Type.Property, config.Type.Value);
            SetDisabledValues(config.Type.Value);
        }

        private void ZMax()
        {
            ValuesExceptions.CheckZMaxTerrain(config.Z_max_relief.Value);
            Listeners.UpdateRelief(config.Z_max_relief.Property, config.Z_max_relief.Value);
        }

        private void Iterations()
        {
            ValuesExceptions.CheckIterations((int)config.Iterations.Value);
            Listeners.UpdateRelief(config.Iterations.Property, config.Iterations.Value);        
        }    

        private void Center()
        {
            ValuesExceptions.CheckCenter(config.Center.Value);
            Listeners.UpdateRelief(config.Center.Property, config.Center.Value);
        }            

        private void Width()
        {
            ValuesExceptions.CheckWidth(config.Width.Value);
            Listeners.UpdateRelief(config.Width.Property, config.Width.Value);
            Listeners.UpdateRelief(config.X_step.Property, config.X_step.Value);
        }

        private void XMax()
        {
            ValuesExceptions.CheckXStep(config.X_step.Value);
            config.N_x.Value = (int) Math.Round(x_max * 1e3 / config.X_step.Value);
            UpdateNx();
        }

        private void XStep()
        {
            ValuesExceptions.CheckXStep(config.X_step.Value);
            config.N_x.Value = (int) Math.Round(x_max * 1e3 / config.X_step.Value);
            ValuesExceptions.CheckNx(config.N_x.Value);
            UpdateXStep();
            UpdateNx();
        }

        private void Nx()
        {
            ValuesExceptions.CheckNegativeNumber(config.N_x.Value);
            config.X_step.Value = x_max * 1e3 / config.N_x.Value;
            ValuesExceptions.CheckXStep(config.X_step.Value);
            add = CheckMaxSize();
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
