using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;


namespace Oragon.Architecture.Web.Ajs
{
	public class SimpleView
	{
		private System.IO.StringWriter HTML { get; set; }
		private int IdentSize;

		private UrlHelper UrlHelper { get; set; }

		public SimpleView(UrlHelper urlHelper)
		{
			this.HTML = new System.IO.StringWriter();
			this.UrlHelper = urlHelper;
			this.IdentSize = 0;
		}


		public void IdentUp()
		{
			this.IdentSize++;
		}

		public void IdentDown()
		{
			this.IdentSize--;
		}

		private string Ident(string text)
		{
			string returnValue = string.Empty;
			for (int i = 0; i < this.IdentSize; i++)
				returnValue += "\t";
			return returnValue + text;
		}

		private SimpleView OpenTag(string tagName)
		{
			this.WriteLine("<{0}>", tagName);
			this.IdentUp();
			return this;
		}

		private SimpleView CloseTag(string tagName)
		{
			this.IdentDown();
			this.WriteLine("</{0}>", tagName);
			return this;
		}

		public SimpleView DefineDoctype()
		{
			this.HTML.WriteLine("<!DOCTYPE html>");
			return this;
		}

		public SimpleView OpenHtml()
		{
			return this.OpenTag("html");
		}

		public SimpleView CloseHtml()
		{
			return this.CloseTag("html");
		}

		public SimpleView OpenHead()
		{
			return this.OpenTag("head");
		}

		public SimpleView CloseHead()
		{
			return this.CloseTag("head");
		}

		public SimpleView OpenBody()
		{
			return this.OpenTag("body");
		}

		public SimpleView CloseBody()
		{
			return this.CloseTag("body");
		}

		public SimpleView Script(string scriptName, params object[] args)
		{
			return this.Script(string.Format(scriptName, args));
		}

		public SimpleView Script(string scriptName)
		{
			scriptName = this.UrlHelper.Content(scriptName);
			this.WriteLine(@"<script type='text/javascript' charset='utf-8' src='{0}'></script>", scriptName);
			return this;
		}

		public SimpleView Stylesheet(string stylesheet, params object[] args)
		{
			return this.Stylesheet(string.Format(stylesheet, args));
		}

		public SimpleView Stylesheet(string stylesheet)
		{
			stylesheet = this.UrlHelper.Content(stylesheet);
			this.WriteLine(@"<link rel='stylesheet' type='text/css' href='{0}'/> ", stylesheet);
			return this;
		}

		public SimpleView OpenScript()
		{
			this.WriteLine("<script type='text/javascript'>");
			this.IdentUp();
			return this;
		}

		public SimpleView CloseScript()
		{
			this.IdentDown();
			this.WriteLine("</script>");
			return this;
		}

		public SimpleView WriteLine(string text)
		{
			this.HTML.WriteLine(this.Ident(text));
			return this;
		}

		public SimpleView WriteLine(string text, params object[] arg)
		{
			this.HTML.WriteLine(this.Ident(text), arg);
			return this;
		}

		public override string ToString()
		{
			return this.HTML.ToString();
		}

		private SimpleView Meta(params string[] parameters)
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

		public SimpleView Meta(string key1, string value1)
		{
			return this.Meta(new string[] { key1, value1 });
		}

		public SimpleView Meta(string key1, string value1, string key2, string value2)
		{
			return this.Meta(new string[] { key1, value1, key2, value2 });
		}

		public SimpleView Meta(string key1, string value1, string key2, string value2, string key3, string value3)
		{
			return this.Meta(new string[] { key1, value1, key2, value2, key3, value3 });
		}

		public SimpleView Meta(string key1, string value1, string key2, string value2, string key3, string value3, string key4, string value4)
		{
			return this.Meta(new string[] { key1, value1, key2, value2, key3, value3, key4, value4 });
		}
	}
}
