using System;

namespace Oragon.Architecture.Web.Owin.OMvc
{
	public class OMvcControllerContext : IDisposable
	{
		#region Private Fields

		private Microsoft.Owin.IOwinContext owinContext;

		#endregion Private Fields

		#region Public Constructors

		public OMvcControllerContext(Microsoft.Owin.IOwinContext owinContext)
		{
			Spring.Threading.LogicalThreadContext.SetData("ControllerContext", owinContext);
		}

		#endregion Public Constructors

		#region Public Methods

		public void Dispose()
		{
			Spring.Threading.LogicalThreadContext.FreeNamedDataSlot("ControllerContext");
		}

		#endregion Public Methods
	}
}