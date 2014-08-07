using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace Oragon.Architecture.Web.Owin.Resolvers
{
	public class SpringDependencyResolver : IDependencyResolver
	{
		#region Public Constructors

		public SpringDependencyResolver(IApplicationContext applicationContext)
		{
			this.ApplicationContext = applicationContext;
		}

		#endregion Public Constructors

		#region Public Properties

		public IApplicationContext ApplicationContext { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public IDependencyScope BeginScope()
		{
			return this;
		}

		public void Dispose()
		{
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

		#endregion Public Methods
	}
}