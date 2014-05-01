using Microsoft.Owin;
using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Middleware
{
	public class MvcMiddlewareOptions
	{
		public IApplicationContext ApplicationContext { get; private set; }
		
		//public List<System.Reflection.Assembly> Assemblies {get; private set;}

		public MvcMiddlewareOptions(IApplicationContext applicationContext)
		{
			this.ApplicationContext = applicationContext;
		}

		

		//public MvcMiddlewareOptions()
		//{
		//	this.Assemblies = new List<System.Reflection.Assembly>();
		//}

		//public void AddAssembly(System.Reflection.Assembly assembly)
		//{
		//	this.Assemblies.Add(assembly);
		//}

		//public void AddAssembly<T>()
		//{
		//	System.Reflection.Assembly assembly = typeof(T).Assembly;
		//	this.AddAssembly(assembly);
		//}


		
	}
}
