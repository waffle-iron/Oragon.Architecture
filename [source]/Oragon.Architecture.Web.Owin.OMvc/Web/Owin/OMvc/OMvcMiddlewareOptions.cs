using Spring.Context;

namespace Oragon.Architecture.Web.Owin.OMvc
{
	public class OMvcMiddlewareOptions
	{
		#region Public Constructors

		public OMvcMiddlewareOptions(IApplicationContext applicationContext)
		{
			this.ApplicationContext = applicationContext;
		}

		#endregion Public Constructors

		#region Public Properties

		public IApplicationContext ApplicationContext { get; private set; }

		#endregion Public Properties
	}
}