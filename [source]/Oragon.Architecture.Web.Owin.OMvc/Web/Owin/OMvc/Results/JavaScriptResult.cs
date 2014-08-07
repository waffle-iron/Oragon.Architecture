using Microsoft.Owin;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class JavaScriptResultExtensions
	{
		#region Public Methods

		public static JavaScriptResult JavaScript(this OMvcController @this, string script)
		{
			return new JavaScriptResult() { JavaScript = script };
		}

		#endregion Public Methods
	}

	public class JavaScriptResult : MvcResult
	{
		#region Public Properties

		public string JavaScript { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override void Execute(IOwinContext context)
		{
			context.Response.ContentType = "application/x-javascript";
			if (this.JavaScript != null)
			{
				context.Response.Write(this.JavaScript);
			}
		}

		#endregion Public Methods
	}
}