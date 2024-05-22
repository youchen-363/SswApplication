using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SswApplication.CSharp.Measurement
{
    internal class MeasString(string property, string value, string unit = "")
    {
        public string Property { get; set; } = property;
        public string Value { get; set; } = value;
        public string Unit { get; set; } = unit;
        public static string[] CreateField(MeasString measurement)
        {
            return
            [
                measurement.Property,
                measurement.Value,
                measurement.Unit
            ];
        }
        
        /// <summary>
        /// Mettre a jour les attributs avec les donnees dans le tableau d'entrée 
        /// </summary>
        /// <param name="data">tableau contenant les données à mettre à jour</param>
        public void UpdateMeasurement(string[] data)
        {
            Property = data[0];
            Value = data[1];
            Unit = data[2];
        }

        /// <summary>
        /// Passer cet objet à une chaine de caracteres contenant tous les attributs et valeurs
        /// </summary>
        /// <returns>chaine de caractere contenant les valeurs de cette classe</returns>
        override
        public string ToString()
        {
            return $"name={Property}, value={Value}, unit={Unit}";
        }
    }
}
