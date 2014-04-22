using Oragon.Architecture.ApplicationHosting.SimpleInjector;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationHostingSimpleInjectorExample
{
	public class SimpleInjectorFactory : ISimpleInjectorFactory
	{
		public Container BuildContainer()
		{
			Container container = new Container();

			container.Register<IAutoStartAppExample, AutoStartAppExample>();

			return container;
		}
	}
}
