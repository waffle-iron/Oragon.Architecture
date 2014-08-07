using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class AppDomainStatistic
	{
		#region Public Properties

		[DataMember]
		public List<AssemblyDescriptor> Assemblies { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public long MonitoringSurvivedMemorySize { get; set; }

		[DataMember]
		public long MonitoringTotalAllocatedMemorySize { get; set; }

		#endregion Public Properties
	}
}