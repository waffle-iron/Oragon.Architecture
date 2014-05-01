using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	public class RegisterServiceController : ApiController
	{
		ApplicationRepository ApplicationRepository { get; set; }

		[HttpPost]
		public Guid Register(HostDescriptor hostDescriptor)
		{
			return this.ApplicationRepository.Register(hostDescriptor);
		}

		[HttpPost]
		public void Unregister(HostDescriptor hostDescriptor)
		{
			this.ApplicationRepository.Unregister(hostDescriptor.ID);
		}
	}
}
