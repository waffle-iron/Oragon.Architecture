using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace Oragon.Architecture.Web.Owin.Resolvers
{
	public class SpringDependencyResolver : IDependencyResolver
	{

		public IApplicationContext ApplicationContext { get; private set; }

		public SpringDependencyResolver(IApplicationContext applicationContext)
		{
			this.ApplicationContext = applicationContext;
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}

		public object GetService(Type serviceType)
		{
			//Console.WriteLine("GetService | " + serviceType.Name);
			return this.GetServices(serviceType).SingleOrDefault();
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			//Console.WriteLine("GetServices | " + serviceType.Name);
			IDictionary<string, object> objectDic = this.ApplicationContext.GetObjectsOfType(serviceType);
			return objectDic.Select(it => it.Value);
		}

		public void Dispose()
		{

		}
	}
}
