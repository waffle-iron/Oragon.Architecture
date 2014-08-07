using Microsoft.Owin;
using System;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public abstract class MvcResult
	{
		#region Public Properties

		public System.Reflection.MethodInfo ActionMethod { get; set; }

		public Type ControllerType { get; set; }

		#endregion Public Properties

		#region Public Methods

		public abstract void Execute(IOwinContext context);

		#endregion Public Methods
	}
}