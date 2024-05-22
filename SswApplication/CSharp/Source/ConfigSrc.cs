using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

/* Unmerged change from project 'SswApplication (net8.0-windows10.0.19041.0)'
Before:
using CsvHelper;
After:
using CsvHelper;
using SswApplication;
using SswApplication.CSharp;
using SswApplication.CSharp.Source;
*/
using SswApplication.CSharp.Functions;
using SswApplication.CSharp.Measurement;

namespace SswApplication.CSharp.Source
{
    internal class ConfigSrc
    {
        //output 
        private MeasNumber n_z; // n_z && z_step && z_max
        private MeasNumber z_step; // z_step && n_z
        private MeasNumber x_s; // - x_s (negative)
        private MeasNumber freq; // frequency && lambda
        private MeasNumber p_tx; // no update files
        private MeasNumber g_tx; // no update files
        private MeasString type; // always CSP
        private MeasNumber z_s; // z_s
        private MeasNumber w0; // width

        // Constructor
        public ConfigSrc()
        {
            freq = new MeasNumber("frequency", 0, "MHz");
            n_z = new MeasNumber("N_z", 0);
            z_step = new MeasNumber("z_step", 0, "m");
            x_s = new MeasNumber("x_s", 0, "m");
            z_s = new MeasNumber("z_s", 0, "m");

            p_tx = new MeasNumber("P_Tx", 0, "W");
            g_tx = new MeasNumber("G_Tx", 0, "dBi");
            w0 = new MeasNumber("W0", 0, "m");
            type = new MeasString("type", "CSP");
        }
        
        public MeasNumber N_z { 
            get => n_z; 
            set 
            { 
                ValuesExceptions.CheckNegativeNumber(value.Value);
                n_z = value; 
            }
        }
        public MeasNumber Z_step { get => z_step; set => z_step = value; }
        public MeasNumber X_s { 
            get => x_s; 
            set 
            {
                ValuesExceptions.CheckXs(value.Value);
                x_s = value;
            }  
        }
        public MeasNumber Frequency { get => freq; set => freq = value; }
        public MeasNumber P_Tx { get => p_tx; set => p_tx = value; }
        public MeasNumber G_Tx { get => g_tx; set => g_tx = value; }
        public MeasString Type {
            get => type;
            set 
            {
                ValuesExceptions.CheckTypeSource(value.Value);
                type = value;
            }  
        }
        public MeasNumber Z_s { get => z_s; set => z_s = value; }
        public MeasNumber W0 { get => w0; set => w0 = value; }

        /// <summary>
        /// en m
        /// </summary>
        /// <returns></returns>
        public double ZMax()
        {
            return N_z.Value * Z_step.Value;
        }

        /// <summary>
        /// passer cet objet à un string 
        /// </summary>
        /// <returns>string contenant les valeurs des attributs de cet objet</returns>
        override
        public string ToString()
        {
            return $"n_z={N_z}, z_step={Z_step}, x_s={X_s}, freq={this.Frequency}, P_tx={P_Tx}, G_Tx={G_Tx}, type={Type}, z_s={Z_s}, W0={W0}"; 
        }
    }
}