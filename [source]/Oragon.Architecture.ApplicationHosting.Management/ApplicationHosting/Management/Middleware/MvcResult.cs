using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;


namespace Oragon.Architecture.ApplicationHosting.Management.Middleware
{
	public abstract class MvcResult
	{
		public abstract void Execute(IOwinContext context);
	}


	/// <summary>Provides a way to return an action result with a specific HTTP response status code and description.</summary>
	public class HttpStatusCodeResult : MvcResult
	{
		/// <summary>Gets the HTTP status code.</summary>
		/// <returns>The HTTP status code.</returns>
		public int StatusCode { get; set; }

		/// <summary>Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpStatusCodeResult" /> class using a status code and status description.</summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="statusDescription">The status description.</param>
		public HttpStatusCodeResult(HttpStatusCode statusCode)
			: this((int)statusCode)
		{
		}
		/// <summary>Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpStatusCodeResult" /> class using a status code and status description.</summary>
		/// <param name="statusCode">The status code.</param>
		public HttpStatusCodeResult(int statusCode)
		{
			this.StatusCode = statusCode;
		}

		public HttpStatusCodeResult()
		{
			this.StatusCode = 200;
		}

		/// <summary>Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.</summary>
		/// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
		public override void Execute(IOwinContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Response.StatusCode = this.StatusCode;
		}
	}


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


	public class ContentResult : MvcResult
	{
		public string Content { get; set; }
		//public Encoding ContentEncoding { get; set; }

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


	public class StreamResult : MvcResult
	{
		public string ContentType { get; set; }

		public System.IO.Stream Stream { get; set; }

		public override void Execute(IOwinContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Response.ContentType = this.ContentType;
			this.Stream.CopyTo(context.Response.Body);
		}
	}
}
