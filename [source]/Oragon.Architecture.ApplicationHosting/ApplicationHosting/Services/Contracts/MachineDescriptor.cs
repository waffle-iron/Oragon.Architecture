using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class MachineDescriptor
	{
		#region Public Properties

		[DataMember]
		public List<string> IpAddressList { get; set; }

		[DataMember]
		public string MachineName { get; set; }

		#endregion Public Properties
	}
}