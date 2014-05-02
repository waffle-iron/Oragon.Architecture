using Oragon.Architecture.Web.Owin.OMvc.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Web.Owin.OMvc
{
	public abstract class OMvcController
	{
		public IEnumerable<MethodInfo> Actions { get; private set; }

		public virtual void Initialize()
		{
			Type type = this.GetType();
			Type mvcResultType = typeof(MvcResult);
			MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(it => mvcResultType.IsAssignableFrom(it.ReturnType)).ToArray();
			this.Actions = new List<MethodInfo>(methods);
		}

		protected Microsoft.Owin.IOwinContext Context { get { return (Microsoft.Owin.IOwinContext)Spring.Threading.LogicalThreadContext.GetData("ControllerContext"); } }

		protected Microsoft.Owin.IOwinRequest Request { get { return this.Context.Request; } }

		protected Microsoft.Owin.IOwinResponse Response { get { return this.Context.Response; } }

		protected System.Net.Http.HttpRequestMessage RequestMessage()
		{
			Microsoft.Owin.IOwinRequest request = this.Request;
			System.Net.Http.HttpRequestMessage httpRequestMessage = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod(request.Method), request.Uri);
			return httpRequestMessage;
		}
		protected System.Web.Http.Routing.UrlHelper UrlHelper()
		{
			return new System.Web.Http.Routing.UrlHelper(this.RequestMessage());
		}
		protected SimpleViewResult SimpleView()
		{
			return new SimpleViewResult(this.UrlHelper());
		}
		protected HttpStatusCodeResult HttpStatusCode(int statusCode)
		{
			return new HttpStatusCodeResult(statusCode);
		}
		protected HttpStatusCodeResult HttpStatusCode(HttpStatusCode statusCode)
		{
			return new HttpStatusCodeResult(statusCode);
		}
		protected JavaScriptResult JavaScript(string script)
		{
			return new JavaScriptResult() { JavaScript = script };
		}
		protected ContentResult Content(string content)
		{
			return new ContentResult() { Content = content };
		}
		protected JsonResult Json(object data)
		{
			return new JsonResult() { Data = data };
		}

		protected RedirectResult Redirect(string url)
		{
			return new RedirectResult(url);
		}

		protected StreamResult Stream(Stream stream, string contentType)
		{
			return new StreamResult() { Stream = stream, ContentType = contentType };
		}
	}
}
