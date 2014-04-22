using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Security.Policy;
using System.Security;
using System.Security.Permissions;

namespace Oragon.Architecture.ApplicationHosting.Ninject
{
	public class NinjectApplicationHost : ApplicationHost<NinjectApplicationHostController>
	{
		public string BootstrapType { get; set; }

		protected override void Setup(NinjectApplicationHostController applicationHostController)
		{
			applicationHostController.Setup(this.BootstrapType);
		}
	}
}
