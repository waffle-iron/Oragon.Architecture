using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Middleware
{
	public abstract class Controller
	{
		public string Name { get; private set; }

		public IEnumerable<MethodInfo> Actions { get; private set; }

		public void Initialize(Type type)
		{
			Type mvcResultType = typeof(MvcResult);
			MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(it => mvcResultType.IsAssignableFrom(it.ReturnType)).ToArray();
			this.Actions = new List<MethodInfo>(methods);
			this.Name = type.Name;
		}

		protected Microsoft.Owin.IOwinContext GetContext() { return (Microsoft.Owin.IOwinContext)Spring.Threading.LogicalThreadContext.GetData("ControllerContext"); }

		protected Microsoft.Owin.IOwinRequest GetRequest() { return this.GetContext().Request; }

		protected Microsoft.Owin.IOwinResponse GetResponse() { return this.GetContext().Response; }


		protected System.Net.Http.HttpRequestMessage RequestMessage()
		{
			Microsoft.Owin.IOwinRequest request = this.GetRequest();
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
	}
}
