using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using Oragon.Architecture.Extensions;
using System.Web.Http.Routing;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class RedirectResultExtensions
	{
		public static RedirectResult Redirect(this OMvcController @this, string url)
		{
			return new RedirectResult(url);
		}
	}

	public class RedirectResult : MvcResult
	{
		public string Url { get; private set; }

		public RedirectResult(string url)
		{
			Contract.Requires(url.IsNotNullOrWhiteSpace());
			this.Url = url;
		}

		public override void Execute(Microsoft.Owin.IOwinContext context)
		{
			Contract.Requires(context.IsNotNull());
			context.Response.Headers.SetValues("Refresh", "0", this.Url);
		}
	}
}
