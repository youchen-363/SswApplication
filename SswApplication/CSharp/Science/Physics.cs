namespace SswApplication.CSharp.Units
{
	internal class Physics
	{
		public static double SpeedOfLight = 299792458;
		public static double Lambda(double frequency)
		{
			return SpeedOfLight / frequency * 1e-6;
		}

		public static double Frequency(double lambda)
		{
			return SpeedOfLight / lambda * 1e-6;
		}
		
	}
}
