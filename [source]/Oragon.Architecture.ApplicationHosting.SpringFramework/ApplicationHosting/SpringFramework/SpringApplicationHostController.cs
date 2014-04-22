using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.SpringFramework
{
	public class SpringApplicationHostController : ApplicationHostController
	{
		public IApplicationContext context;

		public override void Start()
		{
			this.context = Spring.Context.Support.ContextRegistry.GetContext();

			foreach (var item in this.context.GetObjects<ILifecycle>().Select(it => it.Value))
			{
				item.Start();
			}
		}

		public override void Stop()
		{
			if (this.context != null)
			{
				foreach (var item in this.context.GetObjects<ILifecycle>().Select(it => it.Value).Reverse())
				{
					item.Stop();
				}
				this.context.Dispose();
				this.context = null;
			}
		}
	}
}
