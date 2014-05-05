using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public abstract class MvcResult
	{
		public abstract void Execute(IOwinContext context);

		public Type ControllerType { get; set; }

		public System.Reflection.MethodInfo ActionMethod { get; set; }
	}
}
