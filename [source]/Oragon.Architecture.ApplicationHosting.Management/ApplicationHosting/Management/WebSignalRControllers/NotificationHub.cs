using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.NotificationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers
{
	[CLSCompliant(false)]
	[HubName("NotificationHub")]
	public class NotificationHub : Hub
	{
		ApplicationRepository ApplicationRepository { get; set; }
		NotificationRepository NotificationRepository { get; set; }

		public override Task OnConnected()
		{
			return base.OnConnected();
		}

		public override Task OnDisconnected()
		{
			return base.OnDisconnected();
		}


		public override Task OnReconnected()
		{
			return base.OnReconnected();
		}


		public const string Group_ManagementUI = "ManagementUI";


		[HubMethodName("RegisterWebManagement")]
		public void RegisterWebManagement(Guid clientID)
		{
			Groups.Add(this.Context.ConnectionId, Group_ManagementUI);

			Clients.OthersInGroup(Group_ManagementUI).receiveMessages(new Oragon.Architecture.ApplicationHosting.Management.Repository.Models.NotificationModel.Notification[]{
				new Oragon.Architecture.ApplicationHosting.Management.Repository.Models.NotificationModel.Notification()
				{
					Date = DateTime.Now,
					ID = Guid.NewGuid(),
					Message = "New Management Console Openned",
					MessageType = "UI|NEW",
					ContentID = clientID.ToString("D")
				}
			});


			IEnumerable<Notification> notifications = this.NotificationRepository.GetMessages(clientID);
			Clients
				.Caller
				.receiveMessages(notifications);

		}
	}
}
