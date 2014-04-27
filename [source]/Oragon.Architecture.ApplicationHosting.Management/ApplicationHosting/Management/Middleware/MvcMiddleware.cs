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

		private IEnumerable<Controller> Controllers { get; set; }


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
			var controlerBaseType = typeof(Controller);

			var query = from assembly in this.Options.Assemblies
						from type in assembly.ExportedTypes
						where
							type.Name.EndsWith("Controller")
							&&
							type.IsAbstract == false
							&&
							controlerBaseType.IsAssignableFrom(type)
						select type;

			this.Controllers = query.Select(type =>
			{
				Controller controller = (Controller)Activator.CreateInstance(type);
				controller.Initialize(type);
				return controller;
			}).ToArray();

		}


		public Task Invoke(EnvironmentVariables environment)
		{
			IOwinContext owinContext = new OwinContext(environment);
			//if (!this.Options.RootPath.HasValue || owinContext.Request.Path.ToString().ToLower().StartsWith(this.Options.RootPath.ToString().ToLower()))
			{
				System.Net.Http.HttpRequestMessage httpRequestMessage = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod(owinContext.Request.Method), owinContext.Request.Uri);
				IHttpRouteData routeInfo = this.HttpConfiguration.Routes.GetRouteData(httpRequestMessage);
				if (routeInfo != null)
				{
					ControllerActionInvoker invoker = new ControllerActionInvoker(this.Controllers, routeInfo, owinContext);
					if (invoker.Invoke())
					{
						return Task.FromResult<int>(0);
					}
				}
				//WelcomePage welcomePage = new WelcomePage();
				//welcomePage.Execute(owinContext);

			}
			return this.Next(environment);
		}
	}
}
