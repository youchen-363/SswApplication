﻿namespace SswApplication.CSharp.Measurement
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
    }
}
