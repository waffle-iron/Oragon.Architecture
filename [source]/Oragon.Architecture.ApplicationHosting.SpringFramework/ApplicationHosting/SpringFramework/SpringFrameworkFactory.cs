namespace Oragon.Architecture.ApplicationHosting.SpringFramework
{
	public class SpringFrameworkFactory : ISpringFrameworkFactory
	{
		#region Public Methods

		public Spring.Context.IApplicationContext CreateContainer()
		{
			var applicationContext = Spring.Context.Support.ContextRegistry.GetContext();
			return applicationContext;
		}

		#endregion Public Methods
	}
}