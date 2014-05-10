using Oragon.Architecture.ApplicationHosting.Management.ViewModel;
using Oragon.Architecture.ApplicationHosting.Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel;


namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	[RoutePrefix("api/OragonAppStore")]
	public class OragonAppStoreController : ApiController
	{
		ApplicationRepository ApplicationRepository { get; set; }
		NotificationRepository NotificationRepository { get; set; }

		[HttpGet]
		[Route("CreateNewTempApplication/{clientID:guid}/")]
		public Guid CreateNewTempApplication(Guid clientID)
		{
			return ApplicationRepository.CreateNewTempApplication();
		}
	}
}
