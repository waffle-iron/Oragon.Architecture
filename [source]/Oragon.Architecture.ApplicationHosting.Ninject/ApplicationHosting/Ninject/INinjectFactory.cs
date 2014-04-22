using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Ninject
{
	public interface INinjectFactory
	{
		IKernel BuildKernel();
	}
}
