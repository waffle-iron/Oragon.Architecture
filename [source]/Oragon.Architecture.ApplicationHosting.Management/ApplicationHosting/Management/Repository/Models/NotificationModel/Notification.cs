using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository.Models.NotificationModel
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class Notification
	{
		#region Public Constructors

		public Notification()
		{
			this.DeliveredToClients = new List<Guid>();
		}

		#endregion Public Constructors

		#region Public Properties

		[DataMember]
		public string ContentID { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		public List<Guid> DeliveredToClients { get; set; }

		[DataMember]
		public Guid ID { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public string MessageType { get; set; }

		#endregion Public Properties
	}
}