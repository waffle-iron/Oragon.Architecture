using Oragon.Architecture.ApplicationHosting.SpringFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationHostingSpringNetExample
{
	public class SpringFrameworkFactory : ISpringFrameworkFactory
	{
		public Spring.Context.IApplicationContext CreateContainer()
		{
			var applicationContext = Spring.Context.Support.ContextRegistry.GetContext();
			return applicationContext;
		}
	}
}
