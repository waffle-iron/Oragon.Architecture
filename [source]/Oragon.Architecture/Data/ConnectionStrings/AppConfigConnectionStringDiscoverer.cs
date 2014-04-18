using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Data.ConnectionStrings
{
	public class AppConfigConnectionStringDiscoverer : IConnectionStringDiscoverer
	{
		private string ConnectionStringKey { get; set; }

		public System.Configuration.ConnectionStringSettings GetConnectionString()
		{
			System.Configuration.ConnectionStringSettings returnValue = ConfigurationManager.ConnectionStrings[this.ConnectionStringKey];

			if (returnValue == null)
				throw new ConfigurationErrorsException(string.Format("Não foi possível identificar a ConnectionString com a chave '{0}'", this.ConnectionStringKey));

			return returnValue;
		}
	}
}
