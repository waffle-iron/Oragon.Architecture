using Oragon.Architecture.ApplicationHosting.Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationHostingNinjectExample
{
	public class NinjectFactory : INinjectFactory
	{
		public Ninject.IKernel BuildKernel()
		{
			var kernel = new Ninject.StandardKernel();
			kernel.Bind<IAutoStartAppExample>().To<AutoStartAppExample>();
			return kernel;
		}
	}
}
