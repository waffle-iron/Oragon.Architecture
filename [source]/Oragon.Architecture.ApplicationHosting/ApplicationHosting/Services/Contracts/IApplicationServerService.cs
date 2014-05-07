using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[ServiceContract]
	public interface IApplicationServerService : IHeartBeatService
	{
		[OperationContract]
		RegisterHostResponseMessage RegisterHost(RegisterHostRequestMessage request);

		[OperationContract]
		UnregisterHostResponseMessage UnregisterHost(UnregisterHostRequestMessage request);

	}

	#region Register RegisterHost

	[Serializable]
	[DataContract(IsReference = true)]
	public class RegisterHostRequestMessage
	{
		[DataMember]
		public HostDescriptor HostDescriptor { get; set; }

		[DataMember]
		public MachineDescriptor MachineDescriptor { get; set; }


	}


	[Serializable]
	[DataContract(IsReference = true)]
	public class RegisterHostResponseMessage
	{
		[DataMember]
		public Guid ClientID { get; set; }
	}
	#endregion

	#region UnregisterHost
	[Serializable]
	[DataContract(IsReference = true)]
	public class UnregisterHostRequestMessage
	{
		[DataMember]
		public Guid ClientID { get; set; }
	}

	[Serializable]
	[DataContract(IsReference = true)]
	public class UnregisterHostResponseMessage
	{

	}
	#endregion
}
