using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel;

namespace Oragon.Architecture.ApplicationHosting.Management.Services
{
	public class ApplicationServerService : IApplicationServerService
	{
		ApplicationRepository ApplicationRepository { get; set; }
		NotificationRepository NotificationRepository { get; set; }

		public void Init()
		{
			this.NotificationRepository.AddMessage(NotificationRepository.NotificationTypes.GenericNotification, "Application Host Initialized", string.Empty);
		}

		public RegisterHostResponseMessage RegisterHost(RegisterHostRequestMessage request)
		{
			Host host = this.ApplicationRepository.Register(request);
			Uri tcpUri = new Uri("net.tcp://{0}:{1}".FormatWith(request.MachineDescriptor.MachineName, request.HostDescriptor.ManagementTcpPort));
			Uri httpUri = new Uri("http://{0}:{1}".FormatWith(request.MachineDescriptor.MachineName, request.HostDescriptor.ManagementHttpPort));
			host.Connect(tcpUri, httpUri);
			this.NotificationRepository.AddMessage(NotificationRepository.NotificationTypes.ApplicationRegistered, "Application Regitered", host.ID.ToString("D"));
			return new RegisterHostResponseMessage() { ClientID = host.ID };
		}

		public UnregisterHostResponseMessage UnregisterHost(UnregisterHostRequestMessage request)
		{
			Host host = this.ApplicationRepository.Unregister(request);
			host.Disconnect();

			this.NotificationRepository.AddMessage(NotificationRepository.NotificationTypes.ApplicationUnregistered, "Application Unregitered", request.ClientID.ToString("D"));
			return new UnregisterHostResponseMessage();
		}

		public void HeartBeat()
		{
			
		}
	}
}
