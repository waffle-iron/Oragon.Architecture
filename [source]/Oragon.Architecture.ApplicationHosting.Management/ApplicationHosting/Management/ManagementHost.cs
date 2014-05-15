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

		private IDisposable webServer;

		[Required]
		public ManagementHostConfiguration Configuration { get; set; }


		public IApplicationContext ApplicationContext { get; set; }


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
			Oragon.Architecture.Threading.ThreadRunner.RunTask(delegate()
			{//Teste
				IEnumerable<string> IPList = this.GetExternalsIps();
				var options = BuildWebAppOptions(IPList);
				this.webServer = WebApp.Start<ManagementHostStartup>(options);
			});

			Oragon.Architecture.Threading.ThreadRunner.RunTask(delegate()
			{//Teste
				System.Threading.Thread.Sleep(1000 * 5);

				var connection = new Microsoft.AspNet.SignalR.Client.HubConnection("http://localhost:7777/");
				var mediator = new Oragon.Architecture.Services.SignalRServices.SignalRMediator<Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers.IHostHub, Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers.IHostHubClient>(connection, new TesteClass(), Oragon.Architecture.Text.FormatStrategy.None, Oragon.Architecture.Text.FormatStrategy.None);
				connection.Start().Wait();
				mediator.Server.Teste1("AAAAA");
				int returnValue = mediator.Server.Teste2("bb");
				Console.WriteLine("Foi");
			});
		}

		public class TesteClass : Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers.IHostHubClient
		{
			public void Teste(string texto, int nome)
			{
				Console.WriteLine(texto);
			}
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
			this.webServer.Dispose();
			this.webServer = null;
		}



	}
}
