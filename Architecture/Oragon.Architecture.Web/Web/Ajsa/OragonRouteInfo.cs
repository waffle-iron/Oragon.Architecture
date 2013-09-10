using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Web.Ajs
{
	public class OragonRouteInfo
	{
		private System.Web.Mvc.Controller controller;

		public OragonRouteInfo(System.Web.Mvc.Controller controller, string appBaseUrl = null)
		{
			this.controller = controller;

			if(string.IsNullOrWhiteSpace(appBaseUrl))
				this.AppBaseUrl = string.Format("{0}://{1}{2}", 
					this.controller.Request.Url.Scheme, 
					this.controller.Request.Url.Authority, 
					this.controller.Url.Content("~/")
				);
			else
				this.AppBaseUrl = appBaseUrl;

			this.Area = (string)this.controller.RouteData.DataTokens["area"];
			this.Controller = (string)this.controller.RouteData.Values["controller"];
			this.Action = (string)this.controller.RouteData.Values["action"];
			this.ID = (string)this.controller.RouteData.Values["id"];
			this.RedirectTo = controller.HttpContext.Request.QueryString["ReturnUrl"];
		}

		public string RedirectTo { get; private set; }

		public string AppBaseUrl { get; private set; }

		public string Area { get; private set; }

		public string Controller { get; private set; }

		public string Action { get; private set; }

		public string ID { get; private set; }
	}
}
