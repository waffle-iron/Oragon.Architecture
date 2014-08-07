//using System;
//using System.Collections.Concurrent;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Web.Http;
//using System.Web.Http.Controllers;
//using System.Web.Http.Dispatcher;
//using System.Web.Http.Properties;
//using System.Web.Http.Routing;
//using System.Collections.Generic;

//namespace Oragon.Architecture.ApplicationHosting.Management
//{
//	internal static class DictionaryExtensions
//	{
//		public static bool TryGetValue<T>(this IDictionary<string, object> collection, string key, out T value)
//		{
//			object obj;
//			if (collection.TryGetValue(key, out obj) && obj is T)
//			{
//				value = (T)((object)obj);
//				return true;
//			}
//			value = default(T);
//			return false;
//		}

// }

// public static class Error { internal static Exception ArgumentNull(string p) { throw new NotImplementedException(); }

// internal static Exception AmbiguousController() { throw new NotImplementedException(); }

// internal static Exception ControllerNameAmbiguous_WithRouteTemplate() { throw new NotImplementedException(); } } public class CandidateAction {
// public HttpActionDescriptor ActionDescriptor { get; set; } public int Order { get; set; } public decimal Precedence { get; set; } public bool
// MatchName(string actionName) { return string.Equals(this.ActionDescriptor.ActionName, actionName, StringComparison.OrdinalIgnoreCase); } public
// bool MatchVerb(HttpMethod method) { return this.ActionDescriptor.SupportedHttpMethods.Contains(method); } internal string DebuggerToString() {
// return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0}, Order={1}, Prec={2}", new object[] { this.ActionDescriptor.ActionName,
// this.Order, this.Precedence }); } }

// /// <summary>Represents a default <see cref="T:System.Web.Http.Dispatcher.IHttpControllerSelector" /> instance for choosing a <see
// cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> given a <see cref="T:System.Net.Http.HttpRequestMessage" />. A different
// implementation can be registered via the <see cref="P:System.Web.Http.HttpConfiguration.Services" />. </summary> public class
// OragonHttpControllerSelector : IHttpControllerSelector { private const string ControllerKey = "controller"; /// <summary>Specifies the suffix
// string in the controller name.</summary> public static readonly string ControllerSuffix = "Controller"; private readonly HttpConfiguration
// _configuration; private readonly HttpControllerTypeCache _controllerTypeCache; private readonly Lazy<ConcurrentDictionary<string,
// HttpControllerDescriptor>> _controllerInfoCache; /// <summary> Initializes a new instance of the <see
// cref="T:System.Web.Http.Dispatcher.OragonHttpControllerSelector" /> class.</summary> /// <param name="configuration">The configuration.</param>
// public OragonHttpControllerSelector() { HttpConfiguration configuration = ManagementHostStartup.WebApiHttpConfiguration; if (configuration == null)
// { throw Error.ArgumentNull("configuration"); } this._controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(new
// Func<ConcurrentDictionary<string, HttpControllerDescriptor>>(this.InitializeControllerInfoCache)); this._configuration = configuration;
// this._controllerTypeCache = new HttpControllerTypeCache(this._configuration); } /// <summary>Selects a <see
// cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> for the given <see cref="T:System.Net.Http.HttpRequestMessage" />.</summary> ///
// <returns>The <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> instance for the given <see
// cref="T:System.Net.Http.HttpRequestMessage" />.</returns> /// <param name="request">The HTTP request message.</param> public virtual
// HttpControllerDescriptor SelectController(HttpRequestMessage request) { if (request == null) { throw Error.ArgumentNull("request"); }
// IHttpRouteData routeData = request.GetRouteData(); HttpControllerDescriptor directRouteController; if (routeData != null) { directRouteController =
// OragonHttpControllerSelector.GetDirectRouteController(routeData); if (directRouteController != null) { return directRouteController; } } string
// controllerName = this.GetControllerName(request); if (string.IsNullOrEmpty(controllerName)) { throw new
// HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, new Exception())); } if
// (this._controllerInfoCache.Value.TryGetValue(controllerName, out directRouteController)) { return directRouteController; } ICollection<Type>
// controllerTypes = this._controllerTypeCache.GetControllerTypes(controllerName); if (controllerTypes.Count == 0) { throw new
// HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, new Exception())); } throw
// OragonHttpControllerSelector.CreateAmbiguousControllerException(request.GetRouteData().Route, controllerName, controllerTypes); } ///
// <summary>Returns a map, keyed by controller string, of all <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> that the selector
// can select. </summary> /// <returns>A map of all <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> that the selector can
// select, or null if the selector does not have a well-defined mapping of <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor"
// />.</returns> public virtual IDictionary<string, HttpControllerDescriptor> GetControllerMapping() { return
// this._controllerInfoCache.Value.ToDictionary((KeyValuePair<string, HttpControllerDescriptor> c) => c.Key, (KeyValuePair<string,
// HttpControllerDescriptor> c) => c.Value, StringComparer.OrdinalIgnoreCase); } /// <summary>Gets the name of the controller for the specified <see
// cref="T:System.Net.Http.HttpRequestMessage" />.</summary> /// <returns>The name of the controller for the specified <see
// cref="T:System.Net.Http.HttpRequestMessage" />.</returns> /// <param name="request">The HTTP request message.</param> public virtual string
// GetControllerName(HttpRequestMessage request) { if (request == null) { throw new Exception(); } IHttpRouteData routeData = request.GetRouteData();
// if (routeData == null) { return null; } object result = null; routeData.Values.TryGetValue("controller", out result); return (string)result; }

// internal static CandidateAction[] GetDirectRouteCandidates(IHttpRouteData routeData) { IEnumerable<IHttpRouteData> subRoutes =
// routeData.GetSubRoutes(); if (subRoutes == null) { return GetDirectRouteCandidates(routeData.Route); } List<CandidateAction> list = new
// List<CandidateAction>(); foreach (IHttpRouteData current in subRoutes) { CandidateAction[] directRouteCandidates =
// GetDirectRouteCandidates(current.Route); if (directRouteCandidates != null) { list.AddRange(directRouteCandidates); } } return list.ToArray(); }

// public static CandidateAction[] GetDirectRouteCandidates(IHttpRoute route) { IDictionary<string, object> dataTokens = route.DataTokens; if
// (dataTokens == null) { return null; } List<CandidateAction> list = new List<CandidateAction>(); HttpActionDescriptor[] array = null;
// HttpActionDescriptor[] array2; if (dataTokens.TryGetValue<HttpActionDescriptor[]>("actions", out array2) && array2 != null && array2.Length > 0) {
// array = array2; } if (array == null) { return null; } int order = 0; int num; if (dataTokens.TryGetValue<int>("order", out num)) { order = num; }
// decimal precedence = 0m; decimal num2; if (dataTokens.TryGetValue<decimal>("precedence", out num2)) { precedence = num2; } HttpActionDescriptor[]
// array3 = array; for (int i = 0; i < array3.Length; i++) { ReflectedHttpActionDescriptor actionDescriptor =
// (ReflectedHttpActionDescriptor)array3[i]; list.Add(new CandidateAction { ActionDescriptor = actionDescriptor, Order = order, Precedence =
// precedence }); } return list.ToArray(); }

//		private static HttpControllerDescriptor GetDirectRouteController(IHttpRouteData routeData)
//		{
//			CandidateAction[] directRouteCandidates = GetDirectRouteCandidates(routeData);
//			if (directRouteCandidates != null)
//			{
//				HttpControllerDescriptor controllerDescriptor = directRouteCandidates[0].ActionDescriptor.ControllerDescriptor;
//				for (int i = 1; i < directRouteCandidates.Length; i++)
//				{
//					CandidateAction candidateAction = directRouteCandidates[i];
//					if (candidateAction.ActionDescriptor.ControllerDescriptor != controllerDescriptor)
//					{
//						throw OragonHttpControllerSelector.CreateDirectRouteAmbiguousControllerException(directRouteCandidates);
//					}
//				}
//				return controllerDescriptor;
//			}
//			return null;
//		}
//		private static Exception CreateDirectRouteAmbiguousControllerException(CandidateAction[] candidates)
//		{
//			HashSet<Type> hashSet = new HashSet<Type>();
//			for (int i = 0; i < candidates.Length; i++)
//			{
//				hashSet.Add(candidates[i].ActionDescriptor.ControllerDescriptor.ControllerType);
//			}
//			StringBuilder stringBuilder = new StringBuilder();
//			foreach (Type current in hashSet)
//			{
//				stringBuilder.AppendLine();
//				stringBuilder.Append(current.FullName);
//			}
//			return Error.AmbiguousController();
//		}
//		private static Exception CreateAmbiguousControllerException(IHttpRoute route, string controllerName, ICollection<Type> matchingTypes)
//		{
//			StringBuilder stringBuilder = new StringBuilder();
//			foreach (Type current in matchingTypes)
//			{
//				stringBuilder.AppendLine();
//				stringBuilder.Append(current.FullName);
//			}
//			return Error.ControllerNameAmbiguous_WithRouteTemplate();
//		}
//		private ConcurrentDictionary<string, HttpControllerDescriptor> InitializeControllerInfoCache()
//		{
//			ConcurrentDictionary<string, HttpControllerDescriptor> concurrentDictionary = new ConcurrentDictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);
//			HashSet<string> hashSet = new HashSet<string>();
//			Dictionary<string, ILookup<string, Type>> cache = this._controllerTypeCache.Cache;
//			foreach (KeyValuePair<string, ILookup<string, Type>> current in cache)
//			{
//				string key = current.Key;
//				foreach (IGrouping<string, Type> current2 in current.Value)
//				{
//					foreach (Type current3 in current2)
//					{
//						if (concurrentDictionary.Keys.Contains(key))
//						{
//							hashSet.Add(key);
//							break;
//						}
//						concurrentDictionary.TryAdd(key, new HttpControllerDescriptor(this._configuration, key, current3));
//					}
//				}
//			}
//			foreach (string current4 in hashSet)
//			{
//				HttpControllerDescriptor httpControllerDescriptor;
//				concurrentDictionary.TryRemove(current4, out httpControllerDescriptor);
//			}
//			return concurrentDictionary;
//		}
//	}
//}