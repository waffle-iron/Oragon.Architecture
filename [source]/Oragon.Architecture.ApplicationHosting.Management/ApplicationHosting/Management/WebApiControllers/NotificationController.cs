using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.NotificationModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	[RoutePrefix("api/Notification")]
	public class NotificationController : ApiController
	{
		#region Private Properties

		private ApplicationRepository ApplicationRepository { get; set; }

		private NotificationRepository NotificationRepository { get; set; }

		#endregion Private Properties

		#region Public Methods

		[HttpPost]
		[Route("GetMessages/{clientID:guid}/")]
		public IEnumerable<Notification> GetMessages(Guid clientID)
		{
			System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));

			IEnumerable<Notification> returnValue = this.NotificationRepository.GetMessages(clientID);

			return returnValue;
		}

		#endregion Public Methods
	}
}