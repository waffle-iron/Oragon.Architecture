using Oragon.Architecture.Web.Owin.OMvc.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Oragon.Architecture.Web.Owin.OMvc
{
	public abstract class OMvcController
	{
		#region Public Properties

		public IEnumerable<MethodInfo> Actions { get; private set; }

		#endregion Public Properties

		#region Protected Properties

		protected Microsoft.Owin.IOwinContext Context { get { return (Microsoft.Owin.IOwinContext)Spring.Threading.LogicalThreadContext.GetData("ControllerContext"); } }

		protected Microsoft.Owin.IOwinRequest Request { get { return this.Context.Request; } }

		protected Microsoft.Owin.IOwinResponse Response { get { return this.Context.Response; } }

		#endregion Protected Properties

		#region Public Methods

		public System.Web.Http.Routing.UrlHelper CreateUrlHelper()
		{
			return new System.Web.Http.Routing.UrlHelper(this.RequestMessage());
		}

		public virtual void Initialize()
		{
			Type type = this.GetType();
			Type mvcResultType = typeof(MvcResult);
			MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(it => mvcResultType.IsAssignableFrom(it.ReturnType)).ToArray();
			this.Actions = new List<MethodInfo>(methods);
		}

		#endregion Public Methods

		#region Protected Methods

		protected System.Net.Http.HttpRequestMessage RequestMessage()
		{
			Microsoft.Owin.IOwinRequest request = this.Request;
			System.Net.Http.HttpRequestMessage httpRequestMessage = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod(request.Method), request.Uri);
			return httpRequestMessage;
		}

		#endregion Protected Methods
	}
}