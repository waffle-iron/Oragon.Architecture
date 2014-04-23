using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.SpringFramework
{

	public interface ISpringFrameworkFactory : IContainerFactory<IApplicationContext> { }
	public class SpringFrameworkApplicationHost : ApplicationHost<SpringFrameworkApplicationHostController, ISpringFrameworkFactory, IApplicationContext> { }
	public class SpringFrameworkApplicationHostController : ApplicationHostController<ISpringFrameworkFactory, IApplicationContext>
	{
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
	}

}
