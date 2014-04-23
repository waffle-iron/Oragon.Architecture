using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.SimpleInjector
{
	public interface ISimpleInjectorFactory : IContainerFactory<Container> { }
	public class SimpleInjectorApplicationHostController : ApplicationHostController<ISimpleInjectorFactory, Container> { }
	public class SimpleInjectorApplicationHost : ApplicationHost<SimpleInjectorApplicationHostController, ISimpleInjectorFactory, Container> { }
	



}




