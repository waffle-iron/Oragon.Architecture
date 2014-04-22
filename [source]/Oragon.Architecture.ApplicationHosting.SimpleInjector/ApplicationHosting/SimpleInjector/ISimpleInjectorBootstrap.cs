using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.SimpleInjector
{
	public interface ISimpleInjectorBootstrap
	{
		void Bootstrap(Container container);
	}
}
