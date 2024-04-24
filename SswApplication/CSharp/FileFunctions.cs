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
using CsvHelper;

namespace SswApplication.CSharp
{
	internal class FileFunctions
	{
		internal static readonly string[] separator = ["\r\n", "\r", "\n"];
		public static string ExecuteExe(string currentDirectory, string filename)
		{
			string output = string.Empty;
			Environment.CurrentDirectory = currentDirectory;
			try
			{
				Process process = new();
				process.StartInfo.FileName = filename;
				process.StartInfo.Arguments = null;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;

				process.Start();
				process.WaitForExit();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
			return output;
		}

		// Lire donnees de csv
		public static string[][] ReadCSV(string path)
		{
			string[][] data = [];
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

		public static void WriteCSV(string path, string[][] data)
		{
            try
            {
				using var writer = new StreamWriter(path);
				using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
				foreach (string[] dataArray in data)
				{
					foreach (string str in dataArray)
					{
						csvWriter.WriteField(str);
					}
					csvWriter.NextRecord();
				}
			}
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

		public static string ConvertCsvDataToString(string[][] data)
		{
			if (data == null) return "";
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
