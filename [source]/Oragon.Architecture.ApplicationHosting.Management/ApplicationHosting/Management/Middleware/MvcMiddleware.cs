using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvironmentVariables = System.Collections.Generic.IDictionary<string, object>;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Controller = Oragon.Architecture.ApplicationHosting.Management.Middleware;
using System.Web.Http.Routing;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Management.Middleware
{
	public class MvcMiddleware
	{
		private AppFunc Next { get; set; }
		private MvcMiddlewareOptions Options { get; set; }
		private System.Web.Http.HttpConfiguration HttpConfiguration { get; set; }
		private IDictionary<string, Controller> Controllers { get; set; }

		public MvcMiddleware(AppFunc next, MvcMiddlewareOptions options, System.Web.Http.HttpConfiguration httpConfiguration)
		{
			if (next == null)
			{
				throw new ArgumentNullException("next");
			}
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			this.Next = next;
			this.Options = options;
			this.HttpConfiguration = httpConfiguration;
			this.MiddlewareInitialize();
		}

		private void MiddlewareInitialize()
		{
			this.Controllers = this.Options.ApplicationContext.GetObjects<Controller>();
		}


		public async Task Invoke(EnvironmentVariables environment)
		{
			IOwinContext owinContext = new OwinContext(environment);
			System.Net.Http.HttpRequestMessage httpRequestMessage = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod(owinContext.Request.Method), owinContext.Request.Uri);
			IHttpRouteData routeInfo = this.HttpConfiguration.Routes.GetRouteData(httpRequestMessage);
			if (routeInfo != null)
			{
				ControllerActionInvoker invoker = new ControllerActionInvoker(this.Controllers, routeInfo, owinContext);
				if (invoker.Invoke())
				{
					await Task.FromResult<int>(0);
				}
			}
			await this.Next(environment);
		}
	}
}
