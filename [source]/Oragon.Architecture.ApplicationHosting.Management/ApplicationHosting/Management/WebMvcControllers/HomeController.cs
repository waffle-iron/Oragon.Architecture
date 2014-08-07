using Oragon.Architecture.Web.Owin.OMvc;
using Oragon.Architecture.Web.Owin.OMvc.Results;
using System;

namespace Oragon.Architecture.ApplicationHosting.Management.WebMvcControllers
{
	public class HomeController : OMvcController
	{
		#region Public Methods

		public MvcResult Index(string style = "classic")
		{
			Func<SimpleViewResult, SimpleViewResult> crisp = (it =>
				it.Stylesheet("/dynRes/extjs/build/packages/ext-theme-crisp/build/resources/ext-theme-crisp-all-debug.css")
				.Script("/dynRes/extjs/build/ext-all.js")
				.Script("/dynRes/extjs/build/packages/ext-theme-crisp/build/ext-theme-crisp.js")
			);

			Func<SimpleViewResult, SimpleViewResult> gray = (it =>
				it.Stylesheet("/dynRes/extjs/build/packages/ext-theme-gray/build/resources/ext-theme-gray-all-debug.css")
				.Script("/dynRes/extjs/build/ext-all.js")
			);

			Func<SimpleViewResult, SimpleViewResult> classic = (it =>
				it.Stylesheet("/dynRes/extjs/build/packages/ext-theme-classic/build/resources/ext-theme-classic-all-debug.css")
				.Script("/dynRes/extjs/build/ext-all.js")
				.Script("/dynRes/extjs/build/packages/ext-theme-classic/build/ext-theme-classic.js")
			);

			var view = this.SimpleView();

			view.DefineDoctype()
			.OpenHtml()
			.OpenHead();
			switch (style)
			{
				case "classic":
					classic(view); break;
				case "gray":
					gray(view); break;
				case "crisp":
					crisp(view); break;
			}
			view.Stylesheet("/dynRes/icons/style.css")
			.OpenScript()
			.WriteLine("var hostUrl = 'http://{0}/';", this.Request.Host.Value)
			.WriteLine("var clientID = '{0}';", Guid.NewGuid())
			.CloseScript()

			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/jquery-2.1.1.js")
			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/jquery.signalR.js")
			.Script("/signalr/hubs")

			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/Radio.js")
			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/Linq.js")
			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/Reflection.js")
			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/AjaxManager.js")
			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/ErrorManager.js")
			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/TimerManager.js")
			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/Framework/WaitingManager.js")
			.Script("/dynRes/ApplicationHosting/Management/WebResources/Script/App.js")
			.CloseHead()
			.OpenBody()
				//.OpenTag("div", new { @class = "headerContainer" })
				//	.OpenTag("div", new { @class = "headerContainer" })
				//.CloseTag("div")
			.CloseBody()
			.CloseHtml();

			return view;
		}

		public MvcResult Test()
		{
			return this.Json(new { number = 1, value = 2 });
		}

		#endregion Public Methods
	}
}