using System;

namespace Oragon.Architecture.Web.Owin.OMvc
{
	public class OMvcControllerContext : IDisposable
	{
		#region Private Fields

		private const string ControllerContextKey = "ControllerContext";

		#endregion Private Fields

		#region Public Constructors

		public OMvcControllerContext(Microsoft.Owin.IOwinContext owinContext)
		{
			Spring.Threading.LogicalThreadContext.SetData(ControllerContextKey, owinContext);
		}

		public static Microsoft.Owin.IOwinContext Current
		{
			get
			{
				return Spring.Threading.LogicalThreadContext.GetData(ControllerContextKey) as Microsoft.Owin.IOwinContext;
			}

		}

		#endregion Public Constructors

		#region Public Methods

		public void Dispose()
		{
			Spring.Threading.LogicalThreadContext.FreeNamedDataSlot(ControllerContextKey);
		}

		#endregion Public Methods
	}
}