using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	[Serializable]
	public class AppDomainStatistic
	{
		public long MonitoringTotalAllocatedMemorySize { get; set; }

		public DateTime Date { get; set; }

		public long MonitoringSurvivedMemorySize { get; set; }
	}
}
