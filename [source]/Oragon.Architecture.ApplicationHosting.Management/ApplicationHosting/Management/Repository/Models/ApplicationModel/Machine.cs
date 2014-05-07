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
	public class Machine
	{
		[DataMember]
		public MachineDescriptor MachineDescriptor { get; set; }

		[DataMember]
		public List<Host> Hosts { get; set; }
	}
}


		