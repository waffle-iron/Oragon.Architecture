﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using Oragon.Architecture.ApplicationHosting.Model;

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

	[DataContract(IsReference = true)]
	public class MachineDescriptor
	{
		[DataMember]
		public string MachineName { get; set; }

		[DataMember]
		public List<string> IPAddressList { get; set; }
	}



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

	[DataContract(IsReference = true)]
	public class ApplicationDescriptor
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string FriendlyName { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public string FactoryType { get; set; }
		[DataMember]
		public string ApplicationConfigurationFile { get; set; }
		[DataMember]
		public string ApplicationBaseDirectory { get; set; }
	}


	[Serializable]
	[DataContract(IsReference = true)]
	public class RegisterHostResponseMessage
	{
		public Guid ClientID { get; set; }
	}
	#endregion

	#region UnregisterHost
	[Serializable]
	[DataContract(IsReference = true)]
	public class UnregisterHostRequestMessage
	{
		public Guid ClientID { get; set; }
	}

	[Serializable]
	[DataContract(IsReference = true)]
	public class UnregisterHostResponseMessage
	{

	}
	#endregion
}
