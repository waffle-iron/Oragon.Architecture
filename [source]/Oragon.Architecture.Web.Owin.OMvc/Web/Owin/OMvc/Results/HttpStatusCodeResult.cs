using Microsoft.Owin;
using System;
using System.Net;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class HttpStatusCodeResultExtensions
	{
		#region Public Methods

		public static HttpStatusCodeResult HttpStatusCode(this OMvcController @this, int statusCode)
		{
			return new HttpStatusCodeResult(statusCode);
		}

		public static HttpStatusCodeResult HttpStatusCode(this OMvcController @this, HttpStatusCode statusCode)
		{
			return new HttpStatusCodeResult(statusCode);
		}

		#endregion Public Methods
	}

	public class HttpStatusCodeResult : MvcResult
	{
		#region Public Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpStatusCodeResult" /> class using a status code and status description.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="statusDescription">The status description.</param>
		public HttpStatusCodeResult(HttpStatusCode statusCode)
			: this((int)statusCode)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpStatusCodeResult" /> class using a status code and status description.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		public HttpStatusCodeResult(int statusCode)
		{
			this.StatusCode = statusCode;
		}

		public HttpStatusCodeResult()
		{
			this.StatusCode = 200;
		}

		#endregion Public Constructors

		#region Public Properties

		/// <summary>
		///     Gets the HTTP status code.
		/// </summary>
		/// <returns>The HTTP status code.</returns>
		public int StatusCode { get; set; }

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		///     Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"
		///     /> class.
		/// </summary>
		/// <param name="context">
		///     The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.
		/// </param>
		public override void Execute(IOwinContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Response.StatusCode = this.StatusCode;
		}

		#endregion Public Methods
	}
}