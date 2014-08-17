using System;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	#region Register RegisterHost

	[Serializable]
	[DataContract(IsReference = true)]
	public class RegisterHostRequestMessage
	{
		#region Public Properties

		[DataMember]
		public HostDescriptor HostDescriptor { get; set; }

		[DataMember]
		public MachineDescriptor MachineDescriptor { get; set; }

		#endregion Public Properties
	}

	[Serializable]
	[DataContract(IsReference = true)]
	public class RegisterHostResponseMessage
	{
		#region Public Properties

		[DataMember]
		public Guid ClientId { get; set; }

		#endregion Public Properties
	}

	#endregion Register RegisterHost

	#region UnregisterHost

	[Serializable]
	[DataContract(IsReference = true)]
	public class UnregisterHostRequestMessage
	{
		#region Public Properties

		[DataMember]
		public Guid ClientId { get; set; }

		#endregion Public Properties
	}

	[Serializable]
	[DataContract(IsReference = true)]
	public class UnregisterHostResponseMessage
	{
	}

	#endregion UnregisterHost
}