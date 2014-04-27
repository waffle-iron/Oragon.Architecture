using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Middleware
{
	public class ControllerContext : IDisposable
	{
		private Microsoft.Owin.IOwinContext owinContext;

		public ControllerContext(Microsoft.Owin.IOwinContext owinContext)
		{
			Spring.Threading.LogicalThreadContext.SetData("ControllerContext", owinContext);
		}


		public void Dispose()
		{
			Spring.Threading.LogicalThreadContext.FreeNamedDataSlot("ControllerContext");
		}
	}
}
