using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SswApplication.CSharp.Source;

namespace SswApplication.CSharp.Functions
{
    internal class ValuesExceptions
    {
        // Propagation
        public static void CheckImageLayer(double imageLayer){
            if (imageLayer < 0 || imageLayer > 0.5) 
            {
                throw new ArgumentException("Image layer must be in [0,0.5] (percentage of the total field size)");
            }
        }

        public static void CheckApodisationSize(double apoZ)
        {
            if (apoZ < 0 || apoZ > 0.5)
            {
                throw new ArgumentException("Apodisation must be in [0,0.5] (percentage of the total field size)");
            }
        }

        public static void CheckGroundType(string type){
            if (type != "None" && type!="PEC" && type != "Dielectric")
            {
                throw new ArgumentException("Invalid ground type. Must be 'None', 'PEC', or 'Dielectric'.");
            }
        }

        public static void CheckMethod(string method)
        {
            if (method != "SSF")
            {
                throw new ArgumentException("Method must be SSF");
            }
        }

        public static void CheckApodisationType(string apoWindow)
        {
            if (apoWindow != "Hanning")
            {
                throw new ArgumentException($"{apoWindow} is not a valid apodisation type");
            }
        }

        public static void CheckFrequency(double freq)
        {
            if (freq<=0 || freq >= 1000000.0)
            {
                throw new ArgumentException($"Frequency should be >0 and <1000000.0");
            }
        }

        public static void CheckZStep(double zstep, double nz)
        {
            if (zstep<=0 || zstep>nz)
            {
                throw new ArgumentException($"Z_step should be in 0<zstep<{nz}");
            }
        }

        public static void CheckNzValue(double nz)
        {
            if (nz<0 || nz> 1000000)
            {
                throw new ArgumentException($"N_z should be between 0 and 1000000");
            }
        }

        public static void CheckNz(double nz, double waveletLevel)
        {
            double n_scaling_fct = Math.Pow(2, waveletLevel);
            double modulo_nz = nz % n_scaling_fct;
            if (modulo_nz != 0)
            {
                throw new ArgumentException($"N_z must be multiple of {n_scaling_fct} = 2^L");
            }
        }

        public static void CheckNx(double nx)
        {
            if (nx<=0 || nx>500000)
            {
                throw new ArgumentException("N_x should be >0 and <=500000");
            }
        }

        public static void CheckXStep(double xstep)
        {
            if (xstep<0 || xstep>50000.0)
            {
                throw new ArgumentException("X step should be between 0 and 50000.0");
            }
        }

        public static void CheckXMax(double xmax)
        {
            if (xmax<=0 || xmax>50000.0)
            {
                throw new ArgumentException("X max should be >0 and <=50000.0");
            }
        }

        public static void CheckWaveletFamily(string name)
        {
            if (name != "sym6")
            {
                throw new ArgumentException("Incorrect wavelet family. only sym6 is available");
            }
        }

        public static void CheckAtmosphereType(string name)
        {
            if (name != "Linear")
            {
                throw new ArgumentException($"Atmosphere type must be 'Linear' but not {name}");
            }
        }

        public static void CheckZMaxRelief(double zmax, double zstep, double nz)
        {
             if (zmax > zstep * nz)
            {
                throw new ArgumentException("Relief is higher than the computation domain!");
            }
        }
        
        public static void CheckPolarisation(string name)
        {
            if (name != "TE" && name != "TM")
            {
                throw new ArgumentException($"Unknown polarisation. Polarisation should be 'TE' or 'TM' but not {name}");
            }
        }

        public static void CheckTurbulence(string turbulence)
        {
            if (turbulence != "N" && turbulence != "Y")
            {
                throw new ArgumentException($"Turbulence must be 'Y' or 'N' but not {turbulence}");
            }
        }

        public static void CheckPyOrCy(string name)
        {
            if (name != "Python" && name != "Cython")
            {
                throw new ArgumentException($"Language must be 'Python' or 'Cython' but not {name}");
            }
        }

        public static void CheckOutputType(string type)
        {
            if (type != "E (dBV/m)" && type != "F (dB)" && type != "S (dBW/m2)")
            {
                throw new ArgumentException($"Output type must be 'E (dBV/m)' or 'F (dB)' or 'S (dBW/m2)' but not {type}");
            }
        }

        // Terrain 
        public static void CheckTerrainType(string type)
        {
            if (type != "Superposed" && type != "Triangle" && type != "Plane") 
            {
                throw new ArgumentException($"Terrain type must be 'Superposed' or 'Triangle' or 'Plane' but not {type}");
            }
        }

        public static void CheckZMaxTerrain(double zmax)
        {
            if (zmax<0 || zmax > 50000.0)
            {
                throw new ArgumentException("Max relief shoule be between 0 and 50000.0");
            }
        }

        public static void CheckIterations(int iteration)
        {
            if (iteration<0 || iteration>100)
            {
                throw new ArgumentException("Iteration should be betwwen 0 and 100");
            }
        }

        public static void CheckCenter(double center)
        {
            if (center<0 || center>5000000.0) 
            {
                throw new ArgumentException("Center should be between 0 and 5000000.0");
            }
        }

        public static void CheckWidth(double width)
        {
            if (width<0 || width> 50000.0)
            {
                throw new ArgumentException("Width should be between 0 and 50000.0");
            }
        }

        public static void CheckZReliefSize(int nx, int size)
        {

            if (nx + 1 != size)
            {
                throw new ArgumentException("Terrain is not compatible with geometry \n" +
                              $" Length {size} instead of {nx + 1}. Please generate terrain again.");
            }
        }

        // Source 
        public static void CheckTypeSource(string type)
        {
            if (type != "CSP")
            {
                throw new ArgumentException($"Source type must be 'CSP' but not {type}");
            }
        }

        public static void CheckZs(double zs, double zmax)
        {
            if (zs <= 0 || zs>zmax)
            {
                throw new ArgumentException($"Altitude z must be between 0 and {zmax}");
            }
        }

        public static void CheckXs(double xs)
        {
            if (xs >= 0 && xs<=-10000000)
            {
                throw new ArgumentException("Source position along x must be <0 and >-10000000");
            }
        }

        public static void CheckNegativeNumber(double value)
        {
            if (value <= 0)
            {
                throw new ArgumentException($"{value} should be a positive number");
            }
        }

        public static void CheckEFieldSize(int nz)
        {
            string[][] eField = FileFunctions.ReadCSV("CodeSource/Source/outputs/", "E_field.csv");
            int n_z = eField.Length;
            if (n_z != nz)
            {
                throw new ArgumentException("N_z value does not match with saved initial field");
            }
        }

    }
}
