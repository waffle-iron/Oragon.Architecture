using Oragon.Architecture.ApplicationHosting.Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationHostingNinjectExample
{
	public class NinjectFactory : INinjectFactory
	{
		#region Public Methods

		public Ninject.IKernel CreateContainer()
		{
			var kernel = new Ninject.StandardKernel();
			kernel.Bind<IAutoStartAppExample>().To<AutoStartAppExample>();
			return kernel;
		}

		#endregion Public Methods
	}
}