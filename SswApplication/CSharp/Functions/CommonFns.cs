﻿using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

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

		public static string ComplexToString(Complex complex)
		{
			string sign = complex.Imaginary < 0 ? "-" : "+";
			return string.Format(CultureInfo.InvariantCulture, "({0:0.000000000000000000e+00}{1}{2:0.000000000000000000e+00}j)", complex.Real, sign, Math.Abs(complex.Imaginary));
		}

		public static MarkupString ReplaceNToBr(string str)
		{
			return (MarkupString)str.Replace("\n", "<br>");
		}

	}
}
