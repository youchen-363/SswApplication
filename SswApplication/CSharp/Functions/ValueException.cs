namespace SswApplication.CSharp.Functions
{
    internal class ValueException
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
            if (type!="PEC")
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

        public static void CheckZStep(double zstep)
        {
            if (zstep<=0 || zstep>50000.0)
            {
                throw new ArgumentException($"Z_step should be 0 < zstep <= 50000.0");
            }
        }

        public static void CheckNz(double nz)
        {
            if (nz<0 || nz> 1000000)
            {
                throw new ArgumentException($"N_z should be 0 < nz < 1000000");
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

        public static void CheckAtmosphereType(string name)
        {
            if (name != "Linear")
            {
                throw new ArgumentException($"Atmosphere type must be 'Linear' but not {name}");
            }
        }

        public static void CheckPolarisation(string name)
        {
            if (name != "TE")
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

        public static void CheckOutputType(string type)
        {
            if (type != "E (dBV/m)")
            {
                throw new ArgumentException($"Output type must be 'E (dBV/m)' or 'F (dB)' or 'S (dBW/m2)' but not {type}");
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
    }
}
