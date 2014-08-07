using SimpleInjector;

namespace Oragon.Architecture.ApplicationHosting.SimpleInjector
{
	public interface ISimpleInjectorFactory : IContainerFactory<Container> { }

	public class SimpleInjectorApplicationHost : ApplicationHost<SimpleInjectorApplicationHostController, ISimpleInjectorFactory, Container> { }

	public class SimpleInjectorApplicationHostController : ApplicationHostController<ISimpleInjectorFactory, Container> { }
}