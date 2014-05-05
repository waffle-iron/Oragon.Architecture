using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Spring.ServiceModel.Support;

namespace Oragon.Architecture.Services.Hosting
{
	public class WcfServiceHost : ServiceHost, IInitializingObject, IDisposable
	{
		public WcfServiceHost(string serviceName, IObjectFactory objectFactory, bool useServiceProxyTypeCache, params Uri[] baseAddresses)
			: base(WcfServiceHost.CreateServiceType(serviceName, objectFactory, useServiceProxyTypeCache), baseAddresses)
		{
		}

		private static System.Type CreateServiceType(string serviceName, IObjectFactory objectFactory, bool useServiceProxyTypeCache)
		{
			if (serviceName.IsNullOrWhiteSpace())
			{
				throw new System.ArgumentException("The service name cannot be null or an empty string.", "serviceName");
			}
			if (objectFactory.IsTypeMatch(serviceName, typeof(System.Type)))
			{
				return objectFactory.GetObject(serviceName) as System.Type;
			}
			return new ServiceProxyTypeBuilder(serviceName, objectFactory, useServiceProxyTypeCache).BuildProxyType();
		}

		public string Name
		{
			get { return "OragonServiceHost"; }
		}

		public void Start()
		{
			this.Open();
		}

		public void Stop()
		{
			this.Close();
		}

		public void AfterPropertiesSet()
		{
			this.Start();
		}

		public void Dispose()
		{
			this.Close();		
		}
		
	}
}
