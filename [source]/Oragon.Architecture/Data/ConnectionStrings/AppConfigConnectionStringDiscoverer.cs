using System.Configuration;
using System.Globalization;

namespace Oragon.Architecture.Data.ConnectionStrings
{
	public class AppConfigConnectionStringDiscoverer : IConnectionStringDiscoverer
	{
		#region Private Properties

		public string ConnectionStringKey { get; set; }

		#endregion Private Properties

		#region Public Methods

		public System.Configuration.ConnectionStringSettings GetConnectionString()
		{
			System.Configuration.ConnectionStringSettings returnValue = ConfigurationManager.ConnectionStrings[this.ConnectionStringKey];

			if (returnValue == null)
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Cannot find ConnectionString with key '{0}'", this.ConnectionStringKey));

			return returnValue;
		}

		#endregion Public Methods
	}
}