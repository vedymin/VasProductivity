using System.Configuration;

namespace VasProductivity.Misc
{
	public static class ConnectionHelper
	{
		public static string CnnVall(string name)
		{
			return ConfigurationManager.ConnectionStrings[name].ConnectionString;
		}
	}
}