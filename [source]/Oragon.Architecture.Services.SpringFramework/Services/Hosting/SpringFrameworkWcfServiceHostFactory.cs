using Oragon.Architecture.Services.Proxy;
using Spring.Objects.Factory;
using Spring.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Services.Hosting
{
	public class SpringFrameworkWcfServiceHostFactory : WcfServiceHostFactory, IFactoryObject, IInitializingObject, IObjectFactoryAware
	{

		public SpringFrameworkWcfServiceHostFactory()
		{
			this.UseServiceProxyTypeCache = true;
		}

		public virtual bool IsSingleton { get { return false; } }

		public virtual IObjectFactory ObjectFactory { get; set; }

		public virtual System.Type ObjectType { get { return typeof(ServiceHost); } }

		public ServiceHost ServiceHost { get; set; }

		public string TargetName { get; set; }

		public bool UseServiceProxyTypeCache { get; set; }

		public virtual void AfterPropertiesSet()
		{
			Type type = ServiceProxyHelper.CreateServiceType(this.TargetName, this.ObjectFactory, this.UseServiceProxyTypeCache);
			this.ServiceHost = this.BuildHost(type);
		}

		public virtual object GetObject()
		{
			return this.ServiceHost;
		}
	}
}
