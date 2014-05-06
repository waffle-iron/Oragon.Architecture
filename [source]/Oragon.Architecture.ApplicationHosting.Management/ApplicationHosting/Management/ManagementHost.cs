using Microsoft.Owin.Hosting;
using Oragon.Architecture;
using Oragon.Architecture.Extensions;
using Owin;
using Spring.Context;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management
{
	public class ManagementHost : IInitializingObject, IDisposable, IApplicationContextAware
	{
		public static ManagementHost Current { get; private set; }


		private IDisposable server;

		[Required]
		public ManagementHostConfiguration Configuration { get; set; }


		public IApplicationContext ApplicationContext { get; set; }

		public Services.ApplicationServerService ApplicationServerServiceInstance { get; set; }

		public ManagementHost()
		{
			ManagementHost.Current = this;
		}

		private IEnumerable<string> GetExternalsIps()
		{
			var iplist = from ipAddress in Dns.GetHostAddresses(Dns.GetHostName())
						 where
							(
								ipAddress.IsIPv4MappedToIPv6
								||
								ipAddress.IsIPv6LinkLocal
								||
								ipAddress.IsIPv6Multicast
								||
								ipAddress.IsIPv6SiteLocal
								||
								ipAddress.IsIPv6Teredo
							) == false
						 select
							 ipAddress.ToString();


			return iplist.ToArray(); //Only IPV4 IP`s
		}


		public void AfterPropertiesSet()
		{
			IEnumerable<string> IPList = this.GetExternalsIps();
			List<Uri> apiEndpoint = BuildApiEndpoints(IPList);
			var ApplicationServerServiceHost = new ApplicationHosting.Services.WcfHost<Services.ApplicationServerService, ApplicationHosting.Services.Contracts.IApplicationServerService>("ApplicationServerService", apiEndpoint.ToArray());
			ApplicationServerServiceHost.Start(this.ApplicationServerServiceInstance);

			var options = BuildWebAppOptions(IPList);
			this.server = WebApp.Start<ManagementHostStartup>(options);
		}

		private List<Uri> BuildApiEndpoints(IEnumerable<string> IPList)
		{
			var apiEndpoint = new List<Uri>();
			apiEndpoint.Add(new Uri("net.tcp://{0}:{1}/".FormatWith(Environment.MachineName, this.Configuration.ApiTcpPort)));
			apiEndpoint.Add(new Uri("http://{0}:{1}/".FormatWith(Environment.MachineName, this.Configuration.ApiHttpPort)));
			return apiEndpoint;
		}

		private StartOptions BuildWebAppOptions(IEnumerable<string> IPList)
		{
			var options = new StartOptions();
			options.ServerFactory = "Microsoft.Owin.Host.HttpListener";
			options.Urls.Add("http://localhost:{0}".FormatWith(this.Configuration.ManagementPort));
			options.Urls.Add("http://127.0.0.1:{0}".FormatWith(this.Configuration.ManagementPort));
			options.Urls.Add("http://{0}:{1}".FormatWith(Environment.MachineName, this.Configuration.ManagementPort));
			if (this.Configuration.AllowRemoteMonitoring)
			{
				foreach (string ipAddress in IPList)
				{
					options.Urls.Add("http://{0}:{1}".FormatWith(ipAddress, this.Configuration.ManagementPort));
				}
			}
			return options;
		}



		bool disposed = false;
		public void Dispose()
		{
			if (this.disposed)
				return;
			this.DisposeChild();
			this.disposed = true;
			GC.SuppressFinalize(this);
		}


		protected virtual void DisposeChild()
		{
			this.server.Dispose();
		}

	}
}
