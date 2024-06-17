using System.Globalization;

namespace SswApplication.CSharp.Measurement
{
    // type des attributs de configuration
    internal class MeasNumber(string property, double value, string unit = "")
    {
        public string Property { get; set; } = property;
        public double Value { get; set; } = value;
        public string Unit { get; set; } = unit;

        public static string[] CreateField(MeasNumber measurement)
        {
            return
            [
                measurement.Property,
                measurement.Value.ToString(CultureInfo.InvariantCulture),
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
            Value = double.Parse(data[1], CultureInfo.InvariantCulture);
            Unit = data[2];
        }

        /*
        /// <summary>
		/// Passer cet objet à une chaine de caracteres contenant tous les attributs et valeurs
		/// </summary>
		/// <returns>chaine de caractere contenant les valeurs de cette classe</returns>
        override
		public string ToString()
        {
            return $"name={Property}, value={Value}, unit={Unit}";
        }
        */
    }
}
