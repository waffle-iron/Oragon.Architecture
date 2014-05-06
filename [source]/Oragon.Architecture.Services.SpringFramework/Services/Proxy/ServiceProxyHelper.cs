using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Spring.ServiceModel.Support;

namespace Oragon.Architecture.Services.Proxy
{
	public static class ServiceProxyHelper
	{
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
