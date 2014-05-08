using Microsoft.Owin.Hosting;
using Oragon.Architecture;
using Oragon.Architecture.ApplicationHosting.Management.Middleware;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.Web.Owin.OMvc;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Oragon.Architecture.ApplicationHosting.Management
{
	public class ManagementHostStartup
	{
		public static HttpConfiguration WebApiHttpConfiguration;


		public void Configuration(Owin.IAppBuilder app)
		{
			this.ConfigureErrorPage(app);
			this.ConfigureWelcomePage(app);
			this.ConfigureWebApi(app);
			this.ConfigureWebMvc(app);
		}

		private void ConfigureWelcomePage(IAppBuilder app)
		{
			app.UseWelcomePage("/welcome");
		}

		private void ConfigureErrorPage(IAppBuilder app)
		{
			app.UseErrorPage(Microsoft.Owin.Diagnostics.ErrorPageOptions.ShowAll);
		}

		private void ConfigureWebApi(IAppBuilder app)
		{
			var httpConfig = new HttpConfiguration();
			httpConfig.DependencyResolver = new Oragon.Architecture.Web.Owin.Resolvers.SpringDependencyResolver(ManagementHost.Current.ApplicationContext);
			//httpConfig.MapHttpAttributeRoutes();
			httpConfig.Routes.MapHttpRoute(
				name: "WebApi", 
				routeTemplate: "api/{controller}/{action}", 
				defaults: new { controller = "Ping", action = RouteParameter.Optional }
				
			);

			httpConfig.Formatters.Clear();
			httpConfig.Formatters.Add(new HtmlMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Net.Http.Formatting.BsonMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Net.Http.Formatting.XmlMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Web.Http.ModelBinding.JQueryMvcFormUrlEncodedFormatter());

			app.UseWebApi(httpConfig);

			ManagementHostStartup.WebApiHttpConfiguration = httpConfig;
		}


		private void ConfigureWebMvc(IAppBuilder app)
		{
			var httpConfig = new HttpConfiguration();
			httpConfig.Routes.MapHttpRoute("WebMvc", "management/{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = RouteParameter.Optional });
			httpConfig.Routes.MapHttpRoute("WebMvc_resjs", "dynRes/{*resourceName}", new { controller = "Resource", action = "LoadFrom", resourceName = RouteParameter.Optional });
			httpConfig.Routes.MapHttpRoute("WebMvcRedirect", "", new { controller = "Redirect", action = "ToHome", resourceName = RouteParameter.Optional });

			var middlewareOptions = new OMvcMiddlewareOptions(ManagementHost.Current.ApplicationContext);
			app.Use<OMvcMiddleware>(middlewareOptions, httpConfig);
		}


	}


}
