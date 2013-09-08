using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Oragon.Architecture.Web.Ajs
{
	public class OperationResult : ActionResult
	{
		public OperationResultModel Data { get; set; }

		public override void ExecuteResult(ControllerContext context)
		{
			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = "application/json";
			response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
			response.Write(JsonHelper.Serialize(this.Data));
		}
	}
}
