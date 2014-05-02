using Oragon.Architecture.Web.Owin.OMvc;
using Oragon.Architecture.Web.Owin.OMvc.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.WebMvcControllers
{
	public class RedirectController : OMvcController
	{
		public MvcResult ToHome()
		{
			return this.Redirect(this.Request.Uri.ToString() + "Management/");
		}
	}
}
