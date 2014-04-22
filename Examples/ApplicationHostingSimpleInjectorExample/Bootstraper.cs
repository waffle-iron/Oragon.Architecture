using Oragon.Architecture.ApplicationHosting.SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationHostingSimpleInjectorExample
{
	public class Bootstraper : ISimpleInjectorBootstrap
	{
		public void Bootstrap(SimpleInjector.Container container)
		{
			container.Register<IAutoStartAppExample, AutoStartAppExample>();
		}
	}
}
