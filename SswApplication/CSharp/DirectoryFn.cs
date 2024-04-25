using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SswApplication.CSharp
{
    internal class DirectoryFn
    {
        private static readonly string exe_dir = AppDomain.CurrentDomain.BaseDirectory;
        public static string project_dir = exe_dir[..exe_dir.IndexOf("bin")];
        public static string current_dir = Environment.CurrentDirectory;
        
        public static void ChangeDirectory(string dir)
        {
            string path = project_dir + dir;
            Environment.CurrentDirectory = path;
        }

    }
}
