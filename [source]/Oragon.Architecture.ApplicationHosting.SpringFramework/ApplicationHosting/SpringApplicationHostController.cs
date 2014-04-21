using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public class SpringApplicationHostController : ApplicationHostController
	{
		private IApplicationContext context;

		public override void Start()
		{
			this.context = Spring.Context.Support.ContextRegistry.GetContext();
		}

		public override void Stop()
		{
			if (this.context != null)
			{
				this.context.Dispose();
				this.context = null;
			}
		}
	}
}
