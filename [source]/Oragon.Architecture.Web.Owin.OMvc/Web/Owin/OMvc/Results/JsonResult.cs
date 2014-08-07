using Microsoft.Owin;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class JsonResultExtensions
	{
		#region Public Methods

		public static JsonResult Json(this OMvcController @this, object data)
		{
			return new JsonResult() { Data = data };
		}

		#endregion Public Methods
	}

	public class JsonResult : MvcResult
	{
		#region Public Properties

		public object Data { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override void Execute(IOwinContext context)
		{
			context.Response.ContentType = "application/json";
			if (this.Data != null)
			{
				var serialized = Oragon.Architecture.Serialization.JsonHelper.Serialize(this.Data);
				context.Response.Write(serialized);
			}
		}

		#endregion Public Methods
	}
}