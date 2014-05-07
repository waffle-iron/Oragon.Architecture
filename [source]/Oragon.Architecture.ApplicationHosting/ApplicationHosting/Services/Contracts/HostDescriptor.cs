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
	public class HostDescriptor
	{
		[DataMember]
		public int ManagementHttpPort { get; set; }

		[DataMember]
		public int ManagementTcpPort { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string FriendlyName { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public List<ApplicationDescriptor> Applications { get; set; }

		[DataMember]
		public int PID { get; set; }
	}
}
