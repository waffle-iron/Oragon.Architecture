using Oragon.Architecture.ApplicationHosting.Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.NotificationModel;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	[RoutePrefix("api/Notification")]
	public class NotificationController : ApiController
	{
		ApplicationRepository ApplicationRepository { get; set; }
		NotificationRepository NotificationRepository { get; set; }

		[HttpPost]
		[Route("GetMessages/{clientID:guid}/")]
		public IEnumerable<Notification> GetMessages(Guid clientID)
		{
			return this.NotificationRepository.GetMessages(clientID);
		}

	}
}
