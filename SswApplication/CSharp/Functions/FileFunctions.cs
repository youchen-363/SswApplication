﻿using System.Diagnostics;
using System.Globalization;
using CsvHelper;

namespace SswApplication.CSharp.Functions
{
    internal class FileFunctions
    {
        internal static readonly string[] separator = ["\r\n", "\r", "\n"];

        internal static (string, string) ExecuteExe(string dir, string filename)
        {
            string output = string.Empty;
            string error = string.Empty;
            DirectoryFn.ChangeDirectory(dir);
            try 
            {
                Process process = new();
                process.StartInfo.FileName = filename;
                process.StartInfo.Arguments = null;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return (output, error);
        }

        // Lire donnees de csv
        internal static string[][] ReadCSV(string dir, string file)
        {
            string[][] data = [];
            DirectoryFn.ChangeDirectory(dir);
            try
            {
                using var csvFile = new StreamReader(file);
                string csvContent = csvFile.ReadToEnd();
                string[] lines = csvContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                data = new string[lines.Length][];
                for (int i = 0; i < lines.Length; i++)
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

        internal static void WriteCSV(string dir, string file, string[][] data)
        {
            try
            {
                DirectoryFn.ChangeDirectory(dir);
                using var writer = new StreamWriter(file);
                using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

                CsvHelper.Configuration.CsvConfiguration conf = new(CultureInfo.InvariantCulture)
                {
                    // Add any desired configuration settings here
                    ShouldQuote = (field) => false
                };

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

		public static void UpdateCSV(string dir, string file, string name, string value)
        {
            string[][] data = ReadCSV(dir, file);
            for (int i = 0; i < data.Length; i++)
            {
                if (string.Equals(data[i][0], name))
                {
                    data[i][1] = value;
                    break;
                }
            }
			FileFunctions.WriteCSV(dir, file, data);
		}

		public static void UpdateCSV(string dir, string file, string name, double value)
        {
            string[][] data = ReadCSV(dir, file);
            for (int i = 0; i < data.Length; i++)
            {
                if (string.Equals(data[i][0], name))
                {
                    data[i][1] = value.ToString(CultureInfo.InvariantCulture);
                }
            }
            FileFunctions.WriteCSV(dir, file, data);
        }
	}
}
