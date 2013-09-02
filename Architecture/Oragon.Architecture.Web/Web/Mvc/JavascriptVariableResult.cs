using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Oragon.Architecture.Web.Mvc
{
	public class JavascriptVariableResult : ActionResult
	{
		private Dictionary<string, object> _variables;

		public JavascriptVariableResult(Dictionary<string, object> variables)
		{
			this._variables = variables;
		}

		public JavascriptVariableResult(string variable, object value)
			: this(new Dictionary<string, object>())
		{
			this._variables.Add(variable, value);
		}

		public JavascriptVariableResult(string variable1, object value1, string variable2, object value2)
			: this(variable1, value1)
		{
			this._variables.Add(variable2, value2);
		}

		public JavascriptVariableResult(string variable1, object value1, string variable2, object value2, string variable3, object value3)
			: this(variable1, value1, variable2, value2)
		{
			this._variables.Add(variable3, value3);
		}

		public JavascriptVariableResult(string variable1, object value1, string variable2, object value2, string variable3, object value3, string variable4, object value4)
			: this(variable1, value1, variable2, value2, variable3, value3)
		{
			this._variables.Add(variable4, value4);
		}

		public JavascriptVariableResult(string variable1, object value1, string variable2, object value2, string variable3, object value3, string variable4, object value4, string variable5, object value5)
			: this(variable1, value1, variable2, value2, variable3, value3, variable4, value4)
		{
			this._variables.Add(variable5, value5);
		}

		public override void ExecuteResult(ControllerContext context)
		{
			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = "text/javascript";
			System.IO.StringWriter writer = new System.IO.StringWriter();
			foreach (KeyValuePair<string, object> item in this._variables)
			{
				writer.WriteLine("/*Begin definition of {0}*/", item.Key);
				if (item.Key.StartsWith("window") == false)
					writer.Write("var ");
				writer.Write(item.Key);
				writer.Write(" = ");
				writer.Write(JsonHelper.Serialize(item.Value));
				writer.WriteLine(";");
				writer.WriteLine("/*End definition of {0}*/", item.Key);
			}
			response.Write(writer.ToString());
		}
	}
}
