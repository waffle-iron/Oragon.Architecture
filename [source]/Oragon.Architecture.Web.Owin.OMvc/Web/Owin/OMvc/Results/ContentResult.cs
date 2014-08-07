using Microsoft.Owin;
using System;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class ContentResultExtensions
	{
		#region Public Methods

		public static ContentResult Content(this OMvcController @this, string content)
		{
			return new ContentResult() { Content = content };
		}

		#endregion Public Methods
	}

	public class ContentResult : MvcResult
	{
		#region Public Properties

		public string Content { get; set; }

		public string ContentType { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override void Execute(IOwinContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (!string.IsNullOrEmpty(this.ContentType))
			{
				context.Response.ContentType = this.ContentType;
			}
			else
				context.Response.ContentType = "text/html";
			if (this.Content != null)
			{
				context.Response.Write(this.Content);
			}
		}

		#endregion Public Methods
	}
}