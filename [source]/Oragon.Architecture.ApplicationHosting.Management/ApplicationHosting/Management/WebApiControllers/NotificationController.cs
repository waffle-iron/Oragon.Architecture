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
	public class NotificationController : ApiController
	{
		ApplicationRepository ApplicationRepository { get; set; }
		NotificationRepository NotificationRepository { get; set; }

		[HttpGet]
		public IEnumerable<string> GetMessages()
		{
			return this.NotificationRepository.GetMessages();
		}

	}
}
