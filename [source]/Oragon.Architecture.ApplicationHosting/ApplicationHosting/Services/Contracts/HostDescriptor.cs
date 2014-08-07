using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class HostDescriptor
	{
		#region Public Properties

		[DataMember]
		public List<ApplicationDescriptor> Applications { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string FriendlyName { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int PID { get; set; }

		#endregion Public Properties
	}
}