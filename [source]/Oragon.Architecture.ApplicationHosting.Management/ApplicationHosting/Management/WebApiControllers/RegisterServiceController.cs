using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	public class RegisterServiceController : ApiController
	{
		ApplicationRepository ApplicationRepository { get; set; }
		NotificationRepository NotificationRepository { get; set; }

		[HttpPost]
		public Guid Register(HostDescriptor hostDescriptor)
		{
			this.NotificationRepository.AddMessage("Host registring with PID:{1} on machine '{0}'".FormatWith(hostDescriptor.MachineName, hostDescriptor.PID));
			Guid returnValue = this.ApplicationRepository.Register(hostDescriptor);
			this.NotificationRepository.AddMessage("Host registred with ID {0}".FormatWith(returnValue));
			return returnValue;
		}

		[HttpPost]
		public void Unregister(HostDescriptor hostDescriptor)
		{
			this.NotificationRepository.AddMessage("Unrgistring host {0}".FormatWith(hostDescriptor.ID));
			this.ApplicationRepository.Unregister(hostDescriptor.ID);
			this.NotificationRepository.AddMessage("Host unregistred");
		}
	}
}
