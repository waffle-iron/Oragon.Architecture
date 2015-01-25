using Oragon.Architecture.ApplicationHosting.SimpleInjector;
using SimpleInjector;

namespace ApplicationHostingSimpleInjectorExample
{
	public class SimpleInjectorFactory : ISimpleInjectorFactory
	{
		#region Public Methods

		public Container CreateContainer()
		{
			var container = new Container();

			container.Register<IAutoStartAppExample, AutoStartAppExample>();

			return container;
		}

		#endregion Public Methods
	}
}