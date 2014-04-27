using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Management.Middleware
{
	public class ControllerActionInvoker
	{
		private IEnumerable<Controller> Controllers;
		private System.Web.Http.Routing.IHttpRouteData RouteInfo;
		private Microsoft.Owin.IOwinContext OwinContext;

		public ControllerActionInvoker(IEnumerable<Controller> controllers, System.Web.Http.Routing.IHttpRouteData routeInfo, Microsoft.Owin.IOwinContext owinContext)
		{
			this.Controllers = controllers;
			this.RouteInfo = routeInfo;
			this.OwinContext = owinContext;
		}

		public bool Invoke()
		{
			var controllerName = (string)RouteInfo.Values["controller"];
			var actionName = (string)RouteInfo.Values["action"];

			Controller currentController = this.GetController();
			if (currentController != null)
			{
				System.Reflection.MethodInfo currentAction = this.GetAction(currentController);
				if (currentAction != null)
				{
					return this.Invoke(currentController, currentAction);
				}
			}
			return false;
		}

		private bool Invoke(Controller currentController, System.Reflection.MethodInfo currentAction)
		{
			IEnumerable<KeyValuePair<string, object>> parameters = this.GetParameters(currentAction);
			object[] bindedParameters = parameters.Select(it => it.Value).ToArray();
			MvcResult result = null;
			using (new ControllerContext(this.OwinContext))
			{
				result = (MvcResult)currentAction.Invoke(currentController, bindedParameters);
			}
			if (result != null)
				result.Execute(this.OwinContext);
			return true;
		}

		private IEnumerable<KeyValuePair<string, object>> GetParameters(System.Reflection.MethodInfo currentAction)
		{
			return currentAction.GetParameters().Select(it => new KeyValuePair<string, object>(it.Name, this.GetParameterValue(it.Name))).ToArray();
		}

		private object GetParameterValue(string parameterName)
		{
			if (this.RouteInfo.Values.ContainsKey(parameterName))
				return RouteInfo.Values[parameterName];
			else
			{
				IList<string> parameters = this.OwinContext.Request.Query.GetValues(parameterName);
				if (parameters.Count == 1)
					return parameters[0];
			}
			return Type.Missing;
		}

		private Controller GetController()
		{
			var controllerName = (string)RouteInfo.Values["controller"];
			var controllerQuery = this.Controllers.Where(it => it.Name.ToLower() == "{0}Controller".FormatWith(controllerName).ToLower());
			var qtd = controllerQuery.Count();
			if (qtd == 1)
			{
				return controllerQuery.Single();
			}
			return null;
		}

		private System.Reflection.MethodInfo GetAction(Controller currentController)
		{
			var actionName = (string)RouteInfo.Values["action"];
			var actionQuery = currentController.Actions.Where(it => it.Name.ToLower() == actionName.ToLower());
			var qtd = actionQuery.Count();
			if (qtd == 1)
			{
				return actionQuery.Single();
			}
			return null;
		}

	}
}
