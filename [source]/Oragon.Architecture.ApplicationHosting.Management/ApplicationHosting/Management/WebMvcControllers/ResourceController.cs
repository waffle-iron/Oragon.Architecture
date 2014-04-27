using Oragon.Architecture.ApplicationHosting.Management.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.WebMvcControllers
{
	public class ResourceController : Controller
	{

		public MvcResult LoadFrom(string resourceName)
		{
			var returnValue = base.Content(resourceName);
			returnValue.ContentType = base.ResolveMimeType(this.Request.Uri);
			return returnValue;
		}
	}
}
