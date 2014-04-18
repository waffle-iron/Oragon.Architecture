using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Data.ConnectionStrings
{
	public class ConfigurableConnectionStringDiscoverer : IConnectionStringDiscoverer
	{
		public string ConnectionString { get; set; }
		
		public string Name { get; set; }
		
		public string ProviderName { get; set; }

		public System.Configuration.ConnectionStringSettings GetConnectionString()
		{
			return new System.Configuration.ConnectionStringSettings(this.Name, this.ConnectionString, this.ProviderName);
		}
	}
}
