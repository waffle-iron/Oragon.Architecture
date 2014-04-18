using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Oragon.Architecture.Data.ConnectionStrings
{

	public class DefaultConStrConfigDiscovery : IConStrConfigDiscovery
	{
		private string ConnectionStringKey { get; set; }

		public System.Configuration.ConnectionStringSettings GetConnectionString()
		{
			System.Configuration.ConnectionStringSettings returnValue = ConfigurationManager.ConnectionStrings[this.ConnectionStringKey];

			if(returnValue == null)
				throw new ConfigurationErrorsException(string.Format("N�o foi poss�vel identificar a ConnectionString com a chave '{0}'", this.ConnectionStringKey));

			return returnValue;
		}

	}
}
