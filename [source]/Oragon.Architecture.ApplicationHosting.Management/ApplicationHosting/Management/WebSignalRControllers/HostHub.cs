using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;

namespace Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers
{
	[CLSCompliant(false)]
	[HubName("HostHub")]
	public class HostHub : Hub, Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers.IHostHub
	{
		#region Private Properties

		private ApplicationRepository ApplicationRepository { get; set; }

		private NotificationRepository NotificationRepository { get; set; }

		#endregion Private Properties

		#region Public Methods

		public RegisterHostResponseMessage RegisterHost(RegisterHostRequestMessage request)
		{
			Host host = this.ApplicationRepository.Register(request);
			host.ConnectionId = this.Context.ConnectionId;

			this.NotificationRepository.AddMessage(NotificationRepository.NotificationTypes.ApplicationRegistered, "Application Regitered", host.ID.ToString("D"));
			return new RegisterHostResponseMessage() { ClientId = host.ID };
		}

		public void Teste1(string argument)
		{
			Console.WriteLine(argument);
			Clients.Caller.Teste("AAAA", 1);
		}

		public int Teste2(string argument)
		{
			return argument.Length;
		}

		public UnregisterHostResponseMessage UnregisterHost(UnregisterHostRequestMessage request)
		{
			Host host = this.ApplicationRepository.Unregister(request);
			this.NotificationRepository.AddMessage(NotificationRepository.NotificationTypes.ApplicationUnregistered, "Application Unregitered", request.ClientId.ToString("D"));
			return new UnregisterHostResponseMessage();
		}

		#endregion Public Methods
	}
}