using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SswApplication.CSharp
{
    // type des attributs de configuration
    internal class Measurement(string property, double value, string unit = "")
    {
        public string Property { get; set; } = property;
        public double Value { get; set; } = value;
        public string Unit { get; set; } = unit;
        public static string[] CreateField(Measurement measurement)
        {
            return
            [
                measurement.Property,
                measurement.Value.ToString(CultureInfo.InvariantCulture),
                measurement.Unit
            ];
        }
        public static Measurement CreateMeasurement(string[] data)
        {
            return new Measurement(data[0], Double.Parse(data[1], CultureInfo.InvariantCulture), data[2]);
        }

		public void UpdateMeasurement(string[] data)
		{
            Property = data[0];
            Value = Double.Parse(data[1], CultureInfo.InvariantCulture);
			Unit = data[2];
		}

		override
        public string ToString()
        {
            return $"name={Property}, value={Value.ToString()}, unit={Unit}";
        }
    }
}
