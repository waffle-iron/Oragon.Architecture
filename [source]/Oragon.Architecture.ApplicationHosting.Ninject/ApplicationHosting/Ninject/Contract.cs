using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Security.Policy;
using System.Security;
using System.Security.Permissions;
using Ninject;

namespace Oragon.Architecture.ApplicationHosting.Ninject
{
	public interface INinjectFactory : IContainerFactory<IKernel> { }
	public class NinjectApplicationHostController : ApplicationHostController<INinjectFactory, IKernel> { }
	public class NinjectApplicationHost : ApplicationHost<NinjectApplicationHostController, INinjectFactory, IKernel> { }

}
