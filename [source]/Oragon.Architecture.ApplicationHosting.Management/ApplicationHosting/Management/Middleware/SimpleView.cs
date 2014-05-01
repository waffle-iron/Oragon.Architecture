using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Management.Middleware
{
	public class SimpleViewResult : MvcResult
	{
		private System.IO.StringWriter HTML { get; set; }
		private int IdentSize;
		private UrlHelper UrlHelper { get; set; }

		public SimpleViewResult(UrlHelper urlHelper)
		{
			this.HTML = new System.IO.StringWriter();
			this.UrlHelper = urlHelper;
			this.IdentSize = 0;
		}

		public SimpleViewResult IdentUp()
		{
			this.IdentSize++;
			return this;
		}

		public SimpleViewResult IdentDown()
		{
			this.IdentSize--;
			return this;
		}

		private string Ident(string text)
		{
			string returnValue = string.Empty;
			for (int i = 0; i < this.IdentSize; i++)
				returnValue += "\t";
			return returnValue + text;
		}


		public SimpleViewResult OpenTag(string tagName, object attrs = null)
		{
			string attributesStr = string.Empty;
			if (attrs != null)
			{
				IDictionary<string, object> attributes = attrs.ToDictionary();
				foreach (KeyValuePair<string, object> item in attributes)
				{
					attributesStr += @"{0}=""{1}"" ".FormatWith(item.Key, item.Value);
				}
				this.WriteLine("<{0} {1}>", tagName, attributesStr);
			}
			else
				this.WriteLine("<{0}>", tagName);
			this.IdentUp();
			return this;
		}

		public SimpleViewResult CloseTag(string tagName)
		{
			this.IdentDown();
			this.WriteLine("</{0}>", tagName);
			return this;
		}

		public SimpleViewResult DefineDoctype()
		{
			this.HTML.WriteLine("<!DOCTYPE html>");
			return this;
		}

		public SimpleViewResult OpenHtml()
		{
			return this.OpenTag("html");
		}

		public SimpleViewResult CloseHtml()
		{
			return this.CloseTag("html");
		}

		public SimpleViewResult OpenHead()
		{
			return this.OpenTag("head");
		}

		public SimpleViewResult CloseHead()
		{
			return this.CloseTag("head");
		}

		public SimpleViewResult OpenBody()
		{
			return this.OpenTag("body");
		}

		public SimpleViewResult CloseBody()
		{
			return this.CloseTag("body");
		}

		public SimpleViewResult Script(string scriptName, params object[] args)
		{
			return this.Script(string.Format(scriptName, args));
		}

		public SimpleViewResult Script(string scriptName)
		{
			//scriptName = this.UrlHelper.Content(scriptName);
			this.WriteLine(@"<script type='text/javascript' charset='utf-8' src='{0}'></script>", scriptName);
			return this;
		}

		public SimpleViewResult Stylesheet(string stylesheet, params object[] args)
		{
			return this.Stylesheet(string.Format(stylesheet, args));
		}

		public SimpleViewResult Stylesheet(string stylesheet)
		{
			//stylesheet = this.UrlHelper.Content(stylesheet);
			this.WriteLine(@"<link rel='stylesheet' type='text/css' href='{0}'/> ", stylesheet);
			return this;
		}

		public SimpleViewResult OpenScript()
		{
			this.WriteLine("<script type='text/javascript'>");
			this.IdentUp();
			return this;
		}

		public SimpleViewResult CloseScript()
		{
			this.IdentDown();
			this.WriteLine("</script>");
			return this;
		}

		public SimpleViewResult WriteLine(string text)
		{
			this.HTML.WriteLine(this.Ident(text));
			return this;
		}

		public SimpleViewResult WriteLine(string text, params object[] arg)
		{
			this.HTML.WriteLine(this.Ident(text), arg);
			return this;
		}

		public override string ToString()
		{
			return this.HTML.ToString();
		}

		private SimpleViewResult Meta(params string[] parameters)
		{
			List<string> propertiesList = new List<string>();
			for (int i = 0; i < parameters.Length; i += 2)
			{
				var key = parameters[i];
				var value = parameters[i + 1];
				propertiesList.Add(string.Format("{0}='{1}'", key, value));
			}
			string propInline = string.Join(" ", propertiesList.ToArray());
			this.HTML.WriteLine("<meta {0} />", propInline);
			return this;
		}

		public SimpleViewResult Meta(string key1, string value1)
		{
			return this.Meta(new string[] { key1, value1 });
		}

		public SimpleViewResult Meta(string key1, string value1, string key2, string value2)
		{
			return this.Meta(new string[] { key1, value1, key2, value2 });
		}

		public SimpleViewResult Meta(string key1, string value1, string key2, string value2, string key3, string value3)
		{
			return this.Meta(new string[] { key1, value1, key2, value2, key3, value3 });
		}

		public SimpleViewResult Meta(string key1, string value1, string key2, string value2, string key3, string value3, string key4, string value4)
		{
			return this.Meta(new string[] { key1, value1, key2, value2, key3, value3, key4, value4 });
		}


		public override void Execute(Microsoft.Owin.IOwinContext context)
		{
			string output = this.ToString();
			context.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Html;
			context.Response.Write(output);
		}
	}
}
