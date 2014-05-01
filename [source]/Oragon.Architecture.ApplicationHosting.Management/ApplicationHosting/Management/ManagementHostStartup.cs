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
			app.UseWelcomePage("/welcome");
		}

		private void ConfigureErrorPage(IAppBuilder app)
		{
			app.UseErrorPage(Microsoft.Owin.Diagnostics.ErrorPageOptions.ShowAll);
		}

		private void ConfigureWebApi(IAppBuilder app)
		{
			var httpConfig = new HttpConfiguration();
			httpConfig.DependencyResolver = new Oragon.Architecture.ApplicationHosting.Management.Middleware.SpringDependencyResolver(ManagementHost.Current.ApplicationContext);
			httpConfig.MapHttpAttributeRoutes();
			httpConfig.Routes.MapHttpRoute("WebApi", "api/{controller}/{action}", new { controller = "Ping", action = RouteParameter.Optional });

			httpConfig.Formatters.Clear();
			httpConfig.Formatters.Add(new Oragon.Architecture.ApplicationHosting.Management.Middleware.HtmlMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Net.Http.Formatting.BsonMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Net.Http.Formatting.XmlMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter());
			httpConfig.Formatters.Add(new System.Web.Http.ModelBinding.JQueryMvcFormUrlEncodedFormatter());

			app.UseWebApi(httpConfig);
		}


		private void ConfigureWebMvc(IAppBuilder app)
		{
			var httpConfig = new HttpConfiguration();
			httpConfig.Routes.MapHttpRoute("WebMvc", "management/{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = RouteParameter.Optional });
			httpConfig.Routes.MapHttpRoute("WebMvc_resjs", "resource/{*resourceName}", new { controller = "Resource", action = "LoadFrom", resourceName = RouteParameter.Optional });

			var middlewareOptions = new MvcMiddlewareOptions(ManagementHost.Current.ApplicationContext);
			app.Use<MvcMiddleware>(middlewareOptions, httpConfig);
		}


	}


}
