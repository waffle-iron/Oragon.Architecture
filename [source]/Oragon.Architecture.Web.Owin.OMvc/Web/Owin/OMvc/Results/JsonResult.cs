using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class JsonResultExtensions
	{
		public static JsonResult Json(this OMvcController @this, object data)
		{
			return new JsonResult() { Data = data };
		}

	}

	public class JsonResult : MvcResult
	{
		public object Data { get; set; }
		public override void Execute(IOwinContext context)
		{
			context.Response.ContentType = "application/json";
			if (this.Data != null)
			{
				var serialized = Oragon.Architecture.Serialization.JsonHelper.Serialize(this.Data);
				context.Response.Write(serialized);
			}
		}
	}
}
