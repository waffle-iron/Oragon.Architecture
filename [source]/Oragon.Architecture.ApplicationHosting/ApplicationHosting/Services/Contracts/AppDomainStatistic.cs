using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class AppDomainStatistic
	{
		[DataMember]
		public long MonitoringTotalAllocatedMemorySize { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public long MonitoringSurvivedMemorySize { get; set; }

		[DataMember]
		public List<AssemblyDescriptor> Assemblies { get; set; }
	}
}
