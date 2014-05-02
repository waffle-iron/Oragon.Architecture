using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Web.Owin.OMvc
{
	public class OMvcControllerContext : IDisposable
	{
		private Microsoft.Owin.IOwinContext owinContext;

		public OMvcControllerContext(Microsoft.Owin.IOwinContext owinContext)
		{
			Spring.Threading.LogicalThreadContext.SetData("ControllerContext", owinContext);
		}

		public void Dispose()
		{
			Spring.Threading.LogicalThreadContext.FreeNamedDataSlot("ControllerContext");
		}
	}
}
