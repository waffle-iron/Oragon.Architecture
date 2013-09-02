using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.AOP.Data.MongoDB
{
	public class MongoDBConnectionString
	{
		public string Key { get; set; }
		public string ConnectionString { get; set; }
		public string Database { get; set; }
	}
}
