using Oragon.Architecture.Extensions;
using Spring.Objects.Factory;
using Spring.ServiceModel.Support;
using System;
using System.ServiceModel;

namespace Oragon.Architecture.Services.WcfServices
{
	public class SpringFrameworkWcfServiceHostFactory : WcfServiceHostFactory, IFactoryObject, IInitializingObject, IObjectFactoryAware
	{
		#region Public Constructors

		public SpringFrameworkWcfServiceHostFactory()
		{
			this.UseServiceProxyTypeCache = true;
		}

		#endregion Public Constructors

		#region Public Properties

		public virtual bool IsSingleton { get { return false; } }

		public virtual IObjectFactory ObjectFactory { get; set; }

		public virtual System.Type ObjectType { get { return typeof(ServiceHost); } }

		public ServiceHost ServiceHost { get; set; }

		public string TargetName { get; set; }

		public bool UseServiceProxyTypeCache { get; set; }

		#endregion Public Properties

		#region Public Methods

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

		public virtual void AfterPropertiesSet()
		{
			Type type = CreateServiceType(this.TargetName, this.ObjectFactory, this.UseServiceProxyTypeCache);
			this.ServiceHost = this.BuildHost(type);
		}

		public virtual object GetObject()
		{
			return this.ServiceHost;
		}

		#endregion Public Methods
	}
}