using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Oragon.Architecture.Data.ConnectionStrings
{
	public interface IConnectionStringDiscoverer
	{
		ConnectionStringSettings GetConnectionString();
	}
}

