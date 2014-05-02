using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public class JavaScriptResult : MvcResult
	{
		public string JavaScript { get; set; }

		public override void Execute(IOwinContext context)
		{
			context.Response.ContentType = "application/x-javascript";
			if (this.JavaScript != null)
			{
				context.Response.Write(this.JavaScript);
			}
		}
	}
}
