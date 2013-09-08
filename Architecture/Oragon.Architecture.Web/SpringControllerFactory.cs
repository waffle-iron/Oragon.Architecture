using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Oragon.Architecture.Web
{
	public class SpringControllerFactory : DefaultControllerFactory
	{

		/// <summary>
		/// Create the controller.
		/// </summary>
		/// <param name="requestContext">Accepts a <see cref="RequestContext"/>.</param>
		/// <param name="controllerName">Accepts a string controller name.</param>
		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			IController controller = null;
			string controllerClassName = string.Format("{0}Controller", controllerName);
			if (SpringApplicationContext.Contains(controllerClassName))
			{
				controller = SpringApplicationContext.Resolve<IController>(controllerClassName);
				this.RequestContext = requestContext;
			}
			else
			{
				controller = base.CreateController(requestContext, controllerName);
			}
			return controller;
		}
		/// <summary>
		/// Releases the controller.
		/// </summary>
		/// <param name="controller">Accepts an <see cref="IController"/></param>
		public override void ReleaseController(IController controller)
		{
			IDisposable disposable = controller as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		public RequestContext RequestContext { get; set; }
	}
}
