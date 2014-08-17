using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Oragon.Architecture.ApplicationHosting.Management.Middleware;
using Oragon.Architecture.Web.Owin.OMvc;
using Owin;
using System.Web.Http;

namespace Oragon.Architecture.ApplicationHosting.Management
{
	public class ManagementHostStartup
	{
		#region Public Fields

		public static HttpConfiguration WebApiHttpConfiguration;

		#endregion Public Fields

		#region Public Methods

		public void Configuration(Owin.IAppBuilder app)
		{
			this.ConfigureSignalR(app);
			this.ConfigureErrorPage(app);
			this.ConfigureWelcomePage(app);
			this.ConfigureWebApi(app);
			this.ConfigureWebMvc(app);
		}

		#endregion Public Methods

		#region Private Methods

		private void ConfigureErrorPage(IAppBuilder app)
		{
			app.UseErrorPage(Microsoft.Owin.Diagnostics.ErrorPageOptions.ShowAll);
		}

		private void ConfigureSignalR(IAppBuilder app)
		{
			var signalRDependencyResolver = ManagementHost.Current.ApplicationContext.GetObject<Oragon.Architecture.Web.SignalR.SpringFramework.SpringDependencyResolver>();
			var hubConfiguration = new HubConfiguration() { EnableDetailedErrors = true, EnableJavaScriptProxies = true, EnableJSONP = true, Resolver = signalRDependencyResolver };

			//app.MapHubs(hubConfiguration);
			app.UseCors(CorsOptions.AllowAll);
			//app.MapSignalR<Oragon.Architecture.Web.SignalR.SpringFramework.SpringPersistentConnection>("/signalr", hubConfiguration);
			app.MapSignalR(hubConfiguration);
			//GlobalHost.DependencyResolver = signalR_DependencyResolver;
		}

		private void ConfigureWebApi(IAppBuilder app)
		{
			var httpConfig = new HttpConfiguration();
			httpConfig.DependencyResolver = new Oragon.Architecture.Web.Owin.Resolvers.SpringDependencyResolver(ManagementHost.Current.ApplicationContext);
			httpConfig.MapHttpAttributeRoutes();
			//httpConfig.Routes.MapHttpRoute(
			//	name: "WebApi",
			//	routeTemplate: "api/{controller}/{action}/{id}",
			//	defaults: new { id = RouteParameter.Optional }

			//);

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
			httpConfig.Routes.MapHttpRoute(
				name: "WebMvc1",
				routeTemplate: "management/{controller}/{action}/",
				defaults: new { controller = "Home", action = "Index" }
			);
			httpConfig.Routes.MapHttpRoute(
				name: "WebMvc2",
				routeTemplate: "management/{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional },
				constraints: new { id = @"\d+" }
			);
			httpConfig.Routes.MapHttpRoute(
				name: "WebMvc_resjs",
				routeTemplate: "dynRes/{*resourceName}",
				defaults: new { controller = "Resource", action = "LoadFrom", resourceName = RouteParameter.Optional }
			);
			httpConfig.Routes.MapHttpRoute(
				name: "WebMvcRedirect",
				routeTemplate: "",
				defaults: new { controller = "Redirect", action = "ToHome", resourceName = RouteParameter.Optional }
			);

			var middlewareOptions = new OMvcMiddlewareOptions(ManagementHost.Current.ApplicationContext);
			app.Use<OMvcMiddleware>(middlewareOptions, httpConfig);
		}

		private void ConfigureWelcomePage(IAppBuilder app)
		{
			app.UseWelcomePage("/welcome");
		}

		#endregion Private Methods
	}
}