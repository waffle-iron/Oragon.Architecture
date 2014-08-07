using Oragon.Architecture.Extensions;
using System.Diagnostics.Contracts;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class RedirectResultExtensions
	{
		#region Public Methods

		public static RedirectResult Redirect(this OMvcController @this, string url)
		{
			return new RedirectResult(url);
		}

		#endregion Public Methods
	}

	public class RedirectResult : MvcResult
	{
		#region Public Constructors

		public RedirectResult(string url)
		{
			Contract.Requires(url.IsNotNullOrWhiteSpace());
			this.Url = url;
		}

		#endregion Public Constructors

		#region Public Properties

		public string Url { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public override void Execute(Microsoft.Owin.IOwinContext context)
		{
			Contract.Requires(context.IsNotNull());
			context.Response.Headers.SetValues("Refresh", "0", this.Url);
		}

		#endregion Public Methods
	}
}