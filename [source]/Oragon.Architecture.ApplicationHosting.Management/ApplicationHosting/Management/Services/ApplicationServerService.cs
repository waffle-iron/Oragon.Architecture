using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Services
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
	public class ApplicationServerService : IApplicationServerService
	{
		ApplicationRepository ApplicationRepository { get; set; }
		NotificationRepository NotificationRepository { get; set; }

		public RegisterHostResponseMessage RegisterHost(RegisterHostRequestMessage request)
		{


			/*
			this.NotificationRepository.AddMessage("Host registring with PID:{1} on machine '{0}'".FormatWith(hostDescriptor.MachineName, hostDescriptor.PID));
			Guid returnValue = this.ApplicationRepository.Register(hostDescriptor);
			this.NotificationRepository.AddMessage("Host registred with ID {0}".FormatWith(returnValue));
			return returnValue;


			
			this.NotificationRepository.AddMessage("Unrgistring host {0}".FormatWith(hostDescriptor.ID));
			this.ApplicationRepository.Unregister(hostDescriptor.ID);
			this.NotificationRepository.AddMessage("Host unregistred");
			*/

			return null;
		}

		public UnregisterHostResponseMessage UnregisterHost(UnregisterHostRequestMessage request)
		{
			return null;
		}

		public void HeartBeat()
		{
			
		}
	}
}
