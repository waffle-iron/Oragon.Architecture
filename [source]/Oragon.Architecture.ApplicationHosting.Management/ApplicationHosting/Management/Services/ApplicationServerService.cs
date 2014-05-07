using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

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
			Guid id = this.ApplicationRepository.Register(request);
			Uri tcpUri = new Uri("net.tcp://{0}:{1}".FormatWith(request.MachineDescriptor.MachineName, request.HostDescriptor.ManagementTcpPort));
			Uri httpUri = new Uri("http://{0}:{1}".FormatWith(request.MachineDescriptor.MachineName, request.HostDescriptor.ManagementHttpPort));
			using (var hostProcessServiceClient = new Oragon.Architecture.ApplicationHosting.Services.WcfClient<IHostProcessService>(serviceName: "HostProcessService", httpEndpointAddress: httpUri, tcpEndpointAddress: tcpUri))
			{
				hostProcessServiceClient.Service.HeartBeat();
				hostProcessServiceClient.Service.CollectStatistics();
			}
			this.NotificationRepository.AddMessage(NotificationRepository.NotificationTypes.ApplicationRegistered, "Application Regitered", id.ToString("D"));
			return new RegisterHostResponseMessage() { ClientID = id };
		}

		public UnregisterHostResponseMessage UnregisterHost(UnregisterHostRequestMessage request)
		{
			this.ApplicationRepository.Unregister(request);
			this.NotificationRepository.AddMessage(NotificationRepository.NotificationTypes.ApplicationUnregistered, "Application Unregitered", request.ClientID.ToString("D"));
			return new UnregisterHostResponseMessage();
		}

		public void HeartBeat()
		{
			
		}
	}
}
