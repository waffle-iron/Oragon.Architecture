using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Data.ConnectionStrings
{
	public class RedisConnectionString
	{
		public string Key { get; set; }
		public string ConnectionString { get; set; }
		public string IsolationKey { get; set; }
	}
}
