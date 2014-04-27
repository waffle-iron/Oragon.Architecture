using Microsoft.Owin.Hosting;
using Oragon.Architecture;
using Oragon.Architecture.Extensions;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MvcMiddlewareOptions = Oragon.Architecture.ApplicationHosting.Management.Middleware.MvcMiddlewareOptions;
using MvcMiddleware = Oragon.Architecture.ApplicationHosting.Management.Middleware.MvcMiddleware;

namespace Oragon.Architecture.ApplicationHosting.Management
{
	public class ManagementHostStartup
	{
		public void Configuration(Owin.IAppBuilder app)
		{
			this.ConfigureErrorPage(app);
			this.ConfigureWelcomePage(app);
			this.ConfigureWebMvc(app);
			this.ConfigureWebApi(app);
		}

		private void ConfigureWelcomePage(IAppBuilder app)
		{
			app.UseWelcomePage("/about");
		}

		private void ConfigureErrorPage(IAppBuilder app)
		{
			app.UseErrorPage();
		}

		private void ConfigureWebApi(IAppBuilder app)
		{
			var configWebApi = new HttpConfiguration();
			configWebApi.MapHttpAttributeRoutes();
			configWebApi.Routes.MapHttpRoute("WebApi", "api/{controller}/{id}", new { controller = "Ping", id = RouteParameter.Optional });
			//configWebApi.Formatters.XmlFormatter.UseXmlSerializer = true;
			//configWebApi.Formatters.JsonFormatter.UseDataContractJsonSerializer = true;
			app.UseWebApi(configWebApi);
		}


		private void ConfigureWebMvc(IAppBuilder app)
		{
			var configWebMvc = new HttpConfiguration();
			configWebMvc.Routes.MapHttpRoute("WebMvc", "{controller}/{action}", new { controller = "Home", action = "Index", id = RouteParameter.Optional });

			var middlewareOptions = new MvcMiddlewareOptions();
			middlewareOptions.RootPath = new Microsoft.Owin.PathString("/management");

			var assemblies = new System.Reflection.Assembly[] { typeof(ManagementHostStartup).Assembly };

			app.Use<MvcMiddleware>(middlewareOptions, configWebMvc, assemblies);
		}


	}


}
