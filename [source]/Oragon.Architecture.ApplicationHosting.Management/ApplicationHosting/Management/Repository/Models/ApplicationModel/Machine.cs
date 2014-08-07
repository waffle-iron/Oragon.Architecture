using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class Machine
	{
		#region Public Properties

		[DataMember]
		public List<Host> Hosts { get; set; }

		[DataMember]
		public MachineDescriptor MachineDescriptor { get; set; }

		#endregion Public Properties
	}
}