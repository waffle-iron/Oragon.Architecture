using Microsoft.Owin.Hosting;
using Oragon.Architecture.Extensions;
using Spring.Context;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Oragon.Architecture.ApplicationHosting.Management
{
	public class ManagementHost : IInitializingObject, IDisposable, IApplicationContextAware
	{
		#region Private Fields

		private bool _disposed = false;

		private IDisposable _webServer;

		#endregion Private Fields

		#region Public Constructors

		public ManagementHost()
		{
			ManagementHost.Current = this;
		}

		#endregion Public Constructors

		#region Public Properties

		public static ManagementHost Current { get; private set; }

		public IApplicationContext ApplicationContext { get; set; }

		[Required]
		public ManagementHostConfiguration Configuration { get; set; }

		#endregion Public Properties

		#region Public Methods

		public void AfterPropertiesSet()
		{
			Oragon.Architecture.Threading.ThreadRunner.RunTask(delegate()
			{//Teste
				IEnumerable<string> ipList = this.GetExternalsIps();
				var options = BuildWebAppOptions(ipList);
				this._webServer = WebApp.Start<ManagementHostStartup>(options);
			});
		}

		public void Dispose()
		{
			if (this._disposed)
				return;
			this.DisposeChild();
			this._disposed = true;
			GC.SuppressFinalize(this);
		}

		#endregion Public Methods

		#region Protected Methods

		protected virtual void DisposeChild()
		{
			this._webServer.Dispose();
			this._webServer = null;
		}

		#endregion Protected Methods

		#region Private Methods

		private StartOptions BuildWebAppOptions(IEnumerable<string> ipList)
		{
			var options = new StartOptions();
			options.ServerFactory = "Microsoft.Owin.Host.HttpListener";
			options.Urls.Add("http://localhost:{0}".FormatWith(this.Configuration.ManagementPort));
			options.Urls.Add("http://127.0.0.1:{0}".FormatWith(this.Configuration.ManagementPort));
			options.Urls.Add("http://{0}:{1}".FormatWith(Environment.MachineName, this.Configuration.ManagementPort));
			if (this.Configuration.AllowRemoteMonitoring)
			{
				foreach (string ipAddress in ipList)
				{
					options.Urls.Add("http://{0}:{1}".FormatWith(ipAddress, this.Configuration.ManagementPort));
				}
			}
			return options;
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

		#endregion Private Methods

		#region Public Classes

		public class TesteClass : Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers.IHostHubClient
		{
			#region Public Methods

			public void Teste(string texto, int nome)
			{
				Console.WriteLine(texto);
			}

			#endregion Public Methods
		}

		#endregion Public Classes
	}
}