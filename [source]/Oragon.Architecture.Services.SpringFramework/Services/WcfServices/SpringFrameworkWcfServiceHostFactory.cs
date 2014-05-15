using Oragon.Architecture.Extensions;
using Spring.Objects.Factory;
using Spring.ServiceModel;
using Spring.ServiceModel.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Services.WcfServices
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
			Type type = CreateServiceType(this.TargetName, this.ObjectFactory, this.UseServiceProxyTypeCache);
			this.ServiceHost = this.BuildHost(type);
		}

		public virtual object GetObject()
		{
			return this.ServiceHost;
		}

		public static System.Type CreateServiceType(string serviceName, IObjectFactory objectFactory, bool useServiceProxyTypeCache)
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
	}
}
