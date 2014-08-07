using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dispatcher;

namespace Oragon.Architecture.Web.Owin.Resolvers
{
	public abstract class ControllerTypeResolver<T> : IHttpControllerTypeResolver, IApplicationContextAware
	{
		#region Public Properties

		public IApplicationContext ApplicationContext { set; get; }

		#endregion Public Properties

		#region Public Methods

		public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
		{
			IDictionary<string, T> objectDic = this.ApplicationContext.GetObjects<T>();
			return objectDic.Select(it => it.Value.GetType()).ToList();
		}

		#endregion Public Methods
	}

	public class SpringOMvcControllerTypeResolver : ControllerTypeResolver<Oragon.Architecture.Web.Owin.OMvc.OMvcController> { }

	public class SpringWebApiControllerTypeResolver : ControllerTypeResolver<System.Web.Http.ApiController> { }
}