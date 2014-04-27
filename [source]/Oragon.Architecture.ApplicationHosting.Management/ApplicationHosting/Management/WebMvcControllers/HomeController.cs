using Oragon.Architecture.ApplicationHosting.Management.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Oragon.Architecture.ApplicationHosting.Management.WebMvcControllers
{
	public class HomeController : Controller
	{
		public MvcResult Index()
		{
			return this.SimpleView()
				.DefineDoctype()
				.OpenHtml()
				.OpenHead()
				.CloseHead()
				.OpenBody()
				.OpenTag("h1", new { @class = "oi" })
				.WriteLine("Oragon Architecture Application Server")
				.CloseTag("h1")
				.OpenTag("h1", new { @class = "oi" })
				.WriteLine("Oragon Architecture Application Server")
				.CloseTag("h1")
				.CloseBody()
				.CloseHtml();
		}

		public MvcResult Test()
		{
			return this.Json(new { number = 1, value = 2 });
		}
	}
}
