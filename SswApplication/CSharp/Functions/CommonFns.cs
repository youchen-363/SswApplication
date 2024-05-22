using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SswApplication.CSharp.Source;

namespace SswApplication.CSharp.Functions
{
	internal class CommonFns
	{
		// passer des string aux nombres complex
		/// <summary>
		/// passer d'un string à nombre complex en type <Complex>
		/// </summary>
		/// <param name="complexString">chaine de caractere d'un nombre complex</param>
		/// <returns>instance du type Complex avec partie reelle et partie imaginaire</returns>
		/// <exception cref="FormatException"></exception>
		public static Complex ParseStringToComplexNumber(string complexString)
		{
			// Define regex pattern to match the real and imaginary parts
			string pattern = @"\((?<real>[-+]?\d+\.\d+e[-+]?\d+)(?<imaginary>[-+]?\d+\.\d+e[-+]?\d+)j\)";

			// Match the pattern using regex
			Match match = Regex.Match(complexString, pattern);

			if (match.Success)
			{
				// Extract and parse the real and imaginary parts from the regex match
				string realPartStr = match.Groups["real"].Value;
				string imaginaryPartStr = match.Groups["imaginary"].Value;

				double realPart = double.Parse(realPartStr, CultureInfo.InvariantCulture);
				double imaginaryPart = double.Parse(imaginaryPartStr, CultureInfo.InvariantCulture);

				return new Complex(realPart, imaginaryPart);
			}
			else
			{
				throw new FormatException("Invalid complex number format: " + complexString);
			}
		}

		public static MarkupString ReplaceNToBr(string str)
		{
			return (MarkupString)str.Replace("\n", "<br>");
		}

		public static string[][] DbToStr(double[][] dtfinaldb)
		{
            string[][] strrrrrr = new string[dtfinaldb.Length][];

            for (int i = 0; i < dtfinaldb.Length; i++)
            {
                int numCols = dtfinaldb[i].Length;
                strrrrrr[i] = new string[numCols]; // Initialize inner array for current row

                // Iterate through each column of the current row
                for (int j = 0; j < numCols; j++)
                {
                    // Convert double value to its string representation and assign to string array
                    strrrrrr[i][j] = dtfinaldb[i][j].ToString();
                }
            }
            
			return strrrrrr;
        }







	}
}
