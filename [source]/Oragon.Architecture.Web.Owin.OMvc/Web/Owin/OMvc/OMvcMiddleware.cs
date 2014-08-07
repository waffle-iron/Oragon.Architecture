using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using EnvironmentVariables = System.Collections.Generic.IDictionary<string, object>;

namespace Oragon.Architecture.Web.Owin.OMvc
{
	public class OMvcMiddleware
	{
		#region Public Constructors

		public OMvcMiddleware(AppFunc next, OMvcMiddlewareOptions options, System.Web.Http.HttpConfiguration httpConfiguration)
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
			this.Controllers = this.Options.ApplicationContext.GetObjects<OMvcController>();
		}

		#endregion Public Constructors

		#region Private Properties

		private IDictionary<string, OMvcController> Controllers { get; set; }

		private System.Web.Http.HttpConfiguration HttpConfiguration { get; set; }

		private AppFunc Next { get; set; }

		private OMvcMiddlewareOptions Options { get; set; }

		#endregion Private Properties

		#region Public Methods

		public async Task Invoke(EnvironmentVariables environment)
		{
			IOwinContext owinContext = new OwinContext(environment);
			System.Net.Http.HttpRequestMessage httpRequestMessage = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod(owinContext.Request.Method), owinContext.Request.Uri);
			IHttpRouteData routeInfo = this.HttpConfiguration.Routes.GetRouteData(httpRequestMessage);
			if (routeInfo != null)
			{
				var invoker = new OMvcControllerActionInvoker(this.Controllers, routeInfo, owinContext);
				if (invoker.Invoke())
				{
					await Task.FromResult<int>(0);
				}
			}
			await this.Next(environment);
		}

		#endregion Public Methods
	}
}