using Oragon.Architecture.Web.Owin.OMvc;
using Oragon.Architecture.Web.Owin.OMvc.Results;

namespace Oragon.Architecture.ApplicationHosting.Management.WebMvcControllers
{
	public class RedirectController : OMvcController
	{
		#region Public Methods

		public MvcResult ToHome()
		{
			return this.Redirect(this.Request.Uri.ToString() + "Management/");
		}

		#endregion Public Methods
	}
}