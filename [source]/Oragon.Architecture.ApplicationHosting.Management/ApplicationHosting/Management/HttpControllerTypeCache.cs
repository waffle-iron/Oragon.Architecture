using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Oragon.Architecture.ApplicationHosting.Management
{
	internal sealed class HttpControllerTypeCache
	{
		private readonly HttpConfiguration _configuration;
		private readonly Lazy<Dictionary<string, ILookup<string, Type>>> _cache;
		internal Dictionary<string, ILookup<string, Type>> Cache
		{
			get
			{
				return this._cache.Value;
			}
		}
		public HttpControllerTypeCache(HttpConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new Exception("configuration");
			}
			this._configuration = configuration;
			this._cache = new Lazy<Dictionary<string, ILookup<string, Type>>>(new Func<Dictionary<string, ILookup<string, Type>>>(this.InitializeCache));
		}
		public ICollection<Type> GetControllerTypes(string controllerName)
		{
			if (string.IsNullOrEmpty(controllerName))
			{
				throw new Exception("controllerName");
			}
			HashSet<Type> hashSet = new HashSet<Type>();
			ILookup<string, Type> lookup;
			if (this._cache.Value.TryGetValue(controllerName, out lookup))
			{
				foreach (IGrouping<string, Type> current in lookup)
				{
					hashSet.UnionWith(current);
				}
			}
			return hashSet;
		}
		private Dictionary<string, ILookup<string, Type>> InitializeCache()
		{
			IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
			IHttpControllerTypeResolver httpControllerTypeResolver = this._configuration.Services.GetHttpControllerTypeResolver();
			ICollection<Type> controllerTypes = httpControllerTypeResolver.GetControllerTypes(assembliesResolver);
			IEnumerable<IGrouping<string, Type>> source = controllerTypes.GroupBy((Type t) => t.Name.Substring(0, t.Name.Length - OragonHttpControllerSelector.ControllerSuffix.Length), StringComparer.OrdinalIgnoreCase);
			return source.ToDictionary((IGrouping<string, Type> g) => g.Key, (IGrouping<string, Type> g) => g.ToLookup((Type t) => t.Namespace ?? string.Empty, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
		}
	}
}
