using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class Host
	{
		[DataMember]
		public Guid ID { get; set; }

		public string ConnectionId { get; set; }

		[DataMember]
		public HostDescriptor HostDescriptor { get; set; }

		public Host()
		{
		}
		
	}
}
