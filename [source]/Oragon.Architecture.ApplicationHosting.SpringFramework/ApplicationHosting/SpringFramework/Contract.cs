using Spring.Context;
using System.Linq;

namespace Oragon.Architecture.ApplicationHosting.SpringFramework
{
	public interface ISpringFrameworkFactory : IContainerFactory<IApplicationContext> { }

	public class SpringFrameworkApplicationHost : ApplicationHost<SpringFrameworkApplicationHostController, ISpringFrameworkFactory, IApplicationContext>
	{
		#region Public Constructors

		public SpringFrameworkApplicationHost()
		{
			this.FactoryType = "Oragon.Architecture.ApplicationHosting.SpringFramework.SpringFrameworkFactory, Oragon.Architecture.ApplicationHosting.SpringFramework";
		}

		#endregion Public Constructors
	}

	public class SpringFrameworkApplicationHostController : ApplicationHostController<ISpringFrameworkFactory, IApplicationContext>
	{
		#region Public Methods

		public override void Start()
		{
			foreach (var item in this.Container.GetObjects<ILifecycle>().Select(it => it.Value))
			{
				item.Start();
			}
		}

		public override void Stop()
		{
			if (this.Container != null)
			{
				foreach (var item in this.Container.GetObjects<ILifecycle>().Select(it => it.Value).Reverse())
				{
					item.Stop();
				}
				this.Container.Dispose();
				this.Container = null;
			}
		}

		#endregion Public Methods
	}
}