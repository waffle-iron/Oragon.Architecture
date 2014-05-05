using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Model
{
	[Serializable]
	public class AppDomainStatistic
	{
		public long MonitoringTotalAllocatedMemorySize { get; set; }

		public DateTime Date { get; set; }

		public long MonitoringSurvivedMemorySize { get; set; }

		public List<AssemblyDescriptor> Assemblies { get; set; }
	}
}
