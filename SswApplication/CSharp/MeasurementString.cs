using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SswApplication.CSharp
{
	internal class MeasurementString(string property, string value, string unit = "")
	{
		public string Property { get; set; } = property;
		public string Value { get; set; } = value;
		public string Unit { get; set; } = unit;
		public static string[] CreateField(MeasurementString measurement)
		{
			return
			[
				measurement.Property,
				measurement.Value,
				measurement.Unit
			];
		}
		public static MeasurementString CreateMeasurement(string[] data)
		{
			return new MeasurementString(data[0], data[1], data[2]);
		}

		public void UpdateMeasurement(string[] data)
		{
			Property = data[0];
			Value = data[1];
			Unit = data[2];
		}

		override
		public string ToString()
		{
			return $"name={Property}, value={Value}, unit={Unit}";
		}
	}
}
