using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class Host
	{
		#region Public Constructors

		public Host()
		{
		}

		#endregion Public Constructors

		#region Public Properties

		public string ConnectionId { get; set; }

		[DataMember]
		public HostDescriptor HostDescriptor { get; set; }

		[DataMember]
		public Guid ID { get; set; }

		#endregion Public Properties
	}
}