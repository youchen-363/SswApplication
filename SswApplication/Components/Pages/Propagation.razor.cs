using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SswApplication.CSharp;
using SswApplication.CSharp.Propagation;
using SswApplication.CSharp.Source;

namespace SswApplication.Components.Pages
{
	public partial class Propagation
	{
        private ConfigurationPropagation config;

        public Propagation()
        {             
            config = ConfigurationPropagation.ExtractInputCSVSource("CodeSource/Propagation/inputs/", "configuration.csv");
        }

        private void LoadData()
        {
            //FileFunctions.ExecuteExe("CodeSource/Propagation/", "main_propagation.exe");
            ConfigurationPropagation.WriteInputPropagation("CodeSource/Propagation/inputs/", "configuration.csv", config);


		}


	}
}
