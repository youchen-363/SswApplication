using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SswApplication.CSharp;

namespace SswApplication.Components.Pages
{
    public partial class CallJavaScriptInDotNet
    {
        private string path = string.Empty;
        private string p1;
        private string p2;
        private string p3;
        private string strr;
        private string str2;
        public CallJavaScriptInDotNet()
        {
			// path = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryFn.ChangeDirectory(@"wwwroot/js/testfiles");

            foreach (string str in Directory.GetFiles(Environment.CurrentDirectory))
            {
                path += str;
            }
            strr = DirectoryFn.project_dir;
            str2 = Environment.CurrentDirectory;

            // path = Assembly.GetExecutingAssembly().CodeBase;
            // path = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            // p1 = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // AppDomain.CurrentDomain.BaseDirectory();
            // path = Process.GetCurrentProcess().MainModule.FileName;

            // p2 = Hosting.HostingEnvironment.ApplicationPhysicalPath;
            // p3 = this.Server.MapPath("");
            // Console.WriteLine("p2 = " + p2);
            // Console.WriteLine("p3 = " + p3);

            /*
            foreach (string str in Directory.GetDirectories("wwwroot"))
            {
                path += str;
            }
            */

            // path = Environment.ProcessPath;
            //path = Environment.SystemDirectory;
            /*
			Environment.CurrentDirectory = Environment.GetEnvironmentVariable("windir");
			DirectoryInfo info = new DirectoryInfo(".");
            path = info.FullName;
            */
            //path = Environment.GetEnvironmentVariable("MY_PROJECT_DIR"); 
        }


        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        public async Task ShowAlertWindow()
        {
            await JSRuntime.InvokeVoidAsync("draw");
        }




    }
}
