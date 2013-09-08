using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Oragon.Architecture.Log;
using Oragon.Architecture.LogEngine.Business.Entity;
using Spring.Threading;
using Oragon.Architecture.Web.Ajs;

namespace Oragon.Architecture.Web.Ajs
{
	public class OragonController : Controller
	{
		OragonRouteInfo routeInfo;
		public OragonRouteInfo RouteInfo { get { return this.routeInfo; } }
		protected Oragon.Architecture.Log.ILogger Logger { get; set; }
		protected IThreadStorage CallContextStorage { get; set; }
		

		public OragonController()
		{
			this.routeInfo = new OragonRouteInfo(this);			
		}

		#region OperationResult Overrides

		public OperationResult OperationResult(object content)
		{
			return this.OperationResult(string.Empty, content);
		}

		public OperationResult OperationResult(string messageText)
		{
			return this.OperationResult(messageText, null);
		}

		public OperationResult OperationResult(string messageText, object content)
		{
			return this.OperationResult(messageText, OperationResultStatus.Sucess, content);
		}

		public OperationResult OperationResult(OperationResultStatus status, object content)
		{
			return this.OperationResult(string.Empty, status, content);
		}

		public OperationResult OperationResult(string messageText, OperationResultStatus status, object content)
		{
			OperationResultModel returnValue = new OperationResultModel()
			{
				Data = content,
				MessageText = messageText,
				Status = status
			};
			return this.OperationResult(returnValue);
		}

		public OperationResult OperationResult(Exception ex)
		{
			List<string> ExceptionTypes = OragonController.ExtractExceptionHierarchyFromException(ex);
			OperationResultModel returnValue = new OperationResultModel()
			{
				Data = ExceptionTypes,
				MessageText = ex.Message,
				Exception = ex,
				Status = OperationResultStatus.Error
			};
			return this.OperationResult(returnValue);
		}

		/// <summary>
		/// Suporte para o tratamento de exceções no OperationResult
		/// </summary>
		/// <param name="exception"></param>
		/// <returns></returns>
		private static List<string> ExtractExceptionHierarchyFromException(Exception exception)
		{
			Type exceptionType = exception.GetType();
			List<string> ExceptionTypes = new List<string>();
			do
			{
				ExceptionTypes.Add(string.Concat(exceptionType.Namespace, ".", exceptionType.Name));
				exceptionType = exceptionType.BaseType;
			} while (exceptionType != typeof(Exception));
			return ExceptionTypes;
		}

		private OperationResult OperationResult(OperationResultModel model)
		{
			return new OperationResult() { Data = model };
		}

		#endregion

		public new ActionResult Json(object data)
		{
			return this.Content(JsonHelper.Serialize(data));
		}

		public SimpleView GetSimpleView()
		{
			return new SimpleView(this);
		}

		
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			this.SetCurrentReturnType(filterContext);
			base.OnActionExecuting(filterContext);
			this.DisableCache();
			this.ConfigureCallContext();
		}

		private void SetCurrentReturnType(ActionExecutingContext filterContext)
		{
			System.Web.Mvc.ReflectedActionDescriptor reflectedActionDescriptor = filterContext.ActionDescriptor as System.Web.Mvc.ReflectedActionDescriptor;
			if (reflectedActionDescriptor != null)
			{
				this.LastReturnType = reflectedActionDescriptor.MethodInfo.ReturnType;
			}
		}

		private void ConfigureCallContext()
		{
			this.CallContextStorage.SetData("RouteInfo.AppBaseUrl", this.RouteInfo.AppBaseUrl);
			this.CallContextStorage.SetData("RouteInfo.Area", this.RouteInfo.Area);
			this.CallContextStorage.SetData("RouteInfo.Controller", this.RouteInfo.Controller);
			this.CallContextStorage.SetData("RouteInfo.Action", this.RouteInfo.Action);
			this.CallContextStorage.SetData("RouteInfo.ID", this.RouteInfo.ID);
		}

		private void DisableCache()
		{
			this.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
			this.HttpContext.Response.Cache.SetValidUntilExpires(false);
			this.HttpContext.Response.Cache.SetRevalidation(System.Web.HttpCacheRevalidation.AllCaches);
			this.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
			this.HttpContext.Response.Cache.SetNoStore();
		}

		#region Exception Management

		private Type LastReturnType = null;

		protected override void OnException(ExceptionContext filterContext)
		{
			if ((this.LastReturnType != null) && (typeof(OperationResult).IsAssignableFrom(this.LastReturnType)))
			{
				if (filterContext.Exception is Oragon.Architecture.Business.BusinessException)
				{
					filterContext.Result = this.OperationResult(filterContext.Exception);
					this.LogBusinessException(filterContext.Exception);//LOG LOG LOG
					filterContext.ExceptionHandled = true;
				}
				else
				{
					this.LogSystemException(filterContext.Exception);//LOG LOG LOG
				}
				
			}
			else
			{
				base.OnException(filterContext);
				this.LogSystemException(filterContext.Exception);//LOG LOG LOG
			}
		}

		#endregion

		#region Log

		private void Log(LogLevel nivelLog, Exception exception)
		{
			this.Logger.Log("Oragon.Architecture.Web.Mvc.OragonController", exception.Message + System.Environment.NewLine + exception.StackTrace,
				nivelLog,
				"Usuario", this.User.Identity.Name == null ? "null" : this.User.Identity.Name,
				"Area", this.RouteInfo.Area == null ? "null" : this.RouteInfo.Area,
				"Controller", this.RouteInfo.Controller == null ? "null" : this.RouteInfo.Controller,
				"Action", this.RouteInfo.Action == null ? "null" : this.RouteInfo.Action,
				"ID", this.RouteInfo.ID == null ? "null" : this.RouteInfo.ID
				);
		}

		private void LogSystemException(Exception exception)
		{
			this.Log(LogLevel.Fatal, exception);
		}

		private void LogBusinessException(Exception exception)
		{
			this.Log(LogLevel.Warn, exception);
		}

		#endregion

		public ActionResult AcessoNegado()
		{
			return this.View("AcessoNegado");
		}

	}

}
