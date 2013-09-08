using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Web.Ajs
{
	public class OragonRouteInfo
	{
		private System.Web.Mvc.Controller controller;

		public OragonRouteInfo(System.Web.Mvc.Controller controller)
		{
			this.controller = controller;
		}

		public string RedirectTo
		{
			get 
			{
				string valueOfReturnUrl = controller.HttpContext.Request.QueryString["ReturnUrl"];
				return valueOfReturnUrl;
			}
		}

		public string AppBaseUrl
		{
			get
			{
				return   string.Format("{0}://{1}{2}", this.controller.Request.Url.Scheme, this.controller.Request.Url.Authority,  this.controller.Url.Content("~/"));
			}
		}

		public string Area
		{
			get
			{
				return (string)this.controller.RouteData.DataTokens["area"];
			}
		}

		public string Controller
		{
			get
			{
				return (string)this.controller.RouteData.Values["controller"];
			}
		}

		public string Action
		{
			get
			{
				return (string)this.controller.RouteData.Values["action"];
			}
		}

		public string ID
		{
			get
			{
				return (string)this.controller.RouteData.Values["id"];
			}
		}
	}
}
