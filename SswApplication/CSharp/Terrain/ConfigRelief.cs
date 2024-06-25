using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Measurement;

namespace SswApplication.CSharp.Terrain
{
    internal class ConfigRelief
    {
        private MeasNumber n_x;
        private MeasNumber x_step;
        private MeasString type;
        private MeasNumber z_max_relief;
        private MeasNumber iterations;
        private MeasNumber width;
        private MeasNumber center;

        public ConfigRelief()
        {
            n_x = new MeasNumber("N_x", 0);
            x_step = new MeasNumber("x_step", 0, "m");
            type = new MeasString ("type", "Superposed");
            z_max_relief = new MeasNumber("z_max_relief", 0, "m");
            iterations = new MeasNumber("iterations", 0, "for superposed terrain");
            width = new MeasNumber("width", 0, "pts");
            center = new MeasNumber("center", 0, "pts");
        }

        public MeasNumber N_x { 
            get => n_x; 
            set 
            {
                ValueException.CheckNegativeNumber(value.Value);
                n_x = value;
            }  
        }
        public MeasNumber X_step { 
            get => x_step;
            set
            {
                ValueException.CheckNegativeNumber(value.Value);
                x_step = value;
            }
        }
        public MeasString Type { 
            get => type; 
            set {
                //ValueException.CheckTerrainType(value.Value);
                type = value;
            }  
        }
        public MeasNumber Z_max_relief { get =>  z_max_relief; set => z_max_relief = value; }
        public MeasNumber Iterations { get =>  iterations; set => iterations = value;}
        public MeasNumber Width { get => width; set => width = value; }
        public MeasNumber Center { get => center; set => center = value; }

        /// <summary>
        /// en km
        /// </summary>
        /// <returns></returns>
        public double XMax()
        {
            return N_x.Value * X_step.Value / 1000;
        }
    }
}
