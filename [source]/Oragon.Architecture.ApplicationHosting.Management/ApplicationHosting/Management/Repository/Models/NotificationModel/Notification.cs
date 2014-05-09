using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository.Models.NotificationModel
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class Notification
	{
		[DataMember]
		public Guid ID { get; set; }

		[DataMember]
		public string ContentID { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public string MessageType { get; set; }


		public List<Guid> DeliveredToClients { get; set; }

		public Notification()
		{
			this.DeliveredToClients = new List<Guid>();
		}
	}
}
