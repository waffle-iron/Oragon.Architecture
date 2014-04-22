using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDepend.Helpers;
using NDepend.Path;
using System.Diagnostics.Contracts;
using System.Security.Policy;
using System.Security;
using System.Security.Permissions;

namespace Oragon.Architecture.ApplicationHosting.SimpleInjector
{
	public class SimpleInjectorHost : ApplicationHost<SimpleInjectorHostController>
	{
		public string BootstrapType { get; set; }


		protected override void Setup(SimpleInjectorHostController applicationHostController)
		{
			applicationHostController.Setup(this.BootstrapType);
		}
	}
}
