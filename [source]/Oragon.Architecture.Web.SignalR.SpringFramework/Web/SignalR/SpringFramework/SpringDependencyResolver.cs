using Microsoft.AspNet.SignalR;
using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Web.SignalR.SpringFramework
{
	public class SpringDependencyResolver : DefaultDependencyResolver, IApplicationContextAware
	{
		public IApplicationContext ApplicationContext { get; set; }

		public override object GetService(Type serviceType)
		{
			return this.GetServices(serviceType).SingleOrDefault() ?? base.GetService(serviceType);
		}

		public override IEnumerable<object> GetServices(Type serviceType)
		{
			IEnumerable<object> localResult = this.ApplicationContext.GetObjectsOfType(serviceType).Select(it => it.Value);
			IEnumerable<object> baseResult = base.GetServices(serviceType);

			if (baseResult != null && baseResult.Any())
				return localResult.Concat(baseResult);
			else
				return localResult;
		}




	}
}
