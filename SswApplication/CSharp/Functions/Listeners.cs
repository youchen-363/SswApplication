namespace SswApplication.CSharp.Functions
{
    internal class Listeners
    {
        public static void UpdateSource(string name, string value)
        {
            string dir = "CodeSource/source/inputs/";
            string file = "configuration.csv";
            FileFunctions.UpdateCSV(dir, file, name, value);
        }

        public static void UpdateSource(string name, double value)
        {
            string dir = "CodeSource/source/inputs/";
            string file = "configuration.csv";
            FileFunctions.UpdateCSV(dir, file, name, value);
        }

        public static void UpdatePropagation(string name, string value)
        {
            string dir = "CodeSource/propagation/inputs/";
            string file = "configuration.csv";
            FileFunctions.UpdateCSV(dir, file, name, value);
        }

        public static void UpdatePropagation(string name, double value)
        {
            string dir = "CodeSource/propagation/inputs/";
            string file = "configuration.csv";
            FileFunctions.UpdateCSV(dir, file, name, value);
        }

        public static void UpdateRelief(string name, string value)
        {
            string dir = "CodeSource/terrain/inputs/";
            string file = "conf_terrain.csv";
            FileFunctions.UpdateCSV(dir, file, name, value);
        }

        public static void UpdateRelief(string name, double value)
        {
            string dir = "CodeSource/terrain/inputs/";
            string file = "conf_terrain.csv";
            FileFunctions.UpdateCSV(dir, file, name, value);
        }
    }
}
