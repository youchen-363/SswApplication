namespace SswApplication.CSharp.Functions
{
    internal class DirectoryFn
    {
        private static readonly string exe_dir = AppDomain.CurrentDomain.BaseDirectory;
        public static string project_dir = exe_dir[..exe_dir.IndexOf("bin")];

        public static void ChangeDirectory(string dir)
        {
            string path = project_dir + dir;
            Environment.CurrentDirectory = path;
        }

    }
}
