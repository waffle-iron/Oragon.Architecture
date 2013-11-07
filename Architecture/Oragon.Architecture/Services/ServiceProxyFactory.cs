using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AopAlliance.Intercept;
using Spring.Aop.Framework;
using Spring.Messaging.Amqp.Rabbit.Connection;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Config;
using Spring.Proxy;
using Spring.Util;

namespace Oragon.Architecture.Services
{
	public class ServiceProxyFactory : IFactoryObject, IInitializingObject, IConfigurableFactoryObject, IMethodInterceptor
	{
		public bool IsSingleton { get { return true; } }
		public IObjectDefinition ProductTemplate { get; set; }
		private object Proxy { get; set; }
		public Type ObjectType { get; set; }
		protected object Service { get; set; }
		protected Type ServiceInterface { get; set; }

		public void AfterPropertiesSet()
		{
			this.Proxy = this.CreateProxy();
		}

		private object CreateProxy()
		{
			ProxyFactory proxy = new ProxyFactory();
			proxy.AddInterface(this.ServiceInterface);
			proxy.AddAdvice( this);
			proxy.Target = this.Service;
			return proxy.GetProxy();
		}
		public object GetObject()
		{
			return this.Proxy;
		}


		public object Invoke(IMethodInvocation invocation)
		{
			return invocation.Proceed();
		}
	}
}
