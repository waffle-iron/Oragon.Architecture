using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace Oragon.Architecture.Web.Owin.Resolvers
{
	public abstract class ControllerTypeResolver<T> : IHttpControllerTypeResolver, IApplicationContextAware
	{
		public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
		{
			IDictionary<string, object> objectDic1 = this.ApplicationContext.GetObjects<object>();


			IDictionary<string, T> objectDic = this.ApplicationContext.GetObjects<T>();
			return objectDic.Select(it => it.Value.GetType()).ToList();
		}

		public IApplicationContext ApplicationContext { set; get; }
	}


	public class SpringWebApiControllerTypeResolver : ControllerTypeResolver<System.Web.Http.ApiController> { }

	public class SpringOMvcControllerTypeResolver : ControllerTypeResolver<Oragon.Architecture.Web.Owin.OMvc.OMvcController> { }
}
