using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Web.Ajs
{
	public class OragonRouteInfo
	{
		public OragonRouteInfo()
		{

		}

		private static string BuildAppBaseUrl(System.Web.Mvc.Controller controller)
		{
			string returnValue = string.Format("{0}://{1}{2}",
						controller.Request.Url.Scheme,
						controller.Request.Url.Authority,
						controller.Url.Content("~/")
					);
			return returnValue;
		}

		public static OragonRouteInfo Create(System.Web.Mvc.Controller controller, string appBaseUrl = null)
		{
			OragonRouteInfo returnValue = new OragonRouteInfo()
			{
				AppBaseUrl = (string.IsNullOrWhiteSpace(appBaseUrl)) ? BuildAppBaseUrl(controller) : appBaseUrl,
				Area = (string)controller.RouteData.DataTokens["area"],
				Controller = (string)controller.RouteData.Values["controller"],
				Action = (string)controller.RouteData.Values["action"],
				ID = (string)controller.RouteData.Values["id"],
				RedirectTo = controller.HttpContext.Request.QueryString["ReturnUrl"]
			};
			return returnValue;
		}

		public string RedirectTo { get; set; }

		public string AppBaseUrl { get; set; }

		public string Area { get; set; }

		public string Controller { get; set; }

		public string Action { get; set; }

		public string ID { get; set; }
	}
}
