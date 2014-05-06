using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management
{
	public class ManagementHostConfiguration
	{
		[Required]
		public int ManagementPort { get; set; }

		[Required]
		public int ApiTcpPort { get; set; }
		[Required]
		public int ApiHttpPort { get; set; }

		[Required]
		public bool AllowRemoteMonitoring { get; set; }

	}
}
