using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.WebMvcControllers
{
	public abstract class Controller
	{
		public IEnumerable<MethodInfo> Actions { get; private set; }

		public void Initialize(Type type)
		{
			Type mvcResultType = typeof(MvcResult);
			MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(it => mvcResultType.IsAssignableFrom(it.ReturnType)).ToArray();
			this.Actions = new List<MethodInfo>(methods);
		}




	}
}
