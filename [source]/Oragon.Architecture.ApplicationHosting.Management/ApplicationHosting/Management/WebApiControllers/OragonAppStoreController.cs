using Oragon.Architecture.ApplicationHosting.Management.Repository;
using System;
using System.Web.Http;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	[RoutePrefix("api/OragonAppStore")]
	public class OragonAppStoreController : ApiController
	{
		#region Private Properties

		private ApplicationRepository ApplicationRepository { get; set; }

		private NotificationRepository NotificationRepository { get; set; }

		#endregion Private Properties

		#region Public Methods

		[HttpGet]
		[Route("CreateNewTempApplication/{clientID:guid}/")]
		public Guid CreateNewTempApplication(Guid clientID)
		{
			return ApplicationRepository.CreateNewTempApplication();
		}

		#endregion Public Methods
	}
}