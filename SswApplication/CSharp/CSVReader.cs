using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Linq;
using System.Globalization;

namespace SswApplication.CSharp
{
	internal class CSVReader
	{
		internal static readonly string[] separator = ["\r\n", "\r", "\n"];

		// cest bon 
		public static string[][] ReadCSV(string path)
		{
			string[][] data = null;
			try
			{
				using var csvFile = new StreamReader(path);
				string csvContent = csvFile.ReadToEnd();
				string[] lines = csvContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				data = new string[lines.Length][];
				for (int i=0; i<lines.Length; i++)
				{
					string[] values = lines[i].Split(',');
					data[i] = values;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
			return data;
		}

		public static string ConvertCsvDataToString(string[][] data)
		{
			if (data == null) return null;
			StringBuilder sb = new();
			foreach (string[] row in data)
			{
				string line = string.Join(",", row);
				sb.AppendLine(line);
			}
			return sb.ToString();
		}

	}
}
