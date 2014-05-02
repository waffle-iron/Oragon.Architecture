using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public class ContentResult : MvcResult
	{
		public string Content { get; set; }

		public string ContentType { get; set; }

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
	}
}
