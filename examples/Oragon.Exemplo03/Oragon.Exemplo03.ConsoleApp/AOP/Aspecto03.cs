using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Exemplo03.ConsoleApp.AOP
{
	public class Aspecto03 : AopAlliance.Intercept.IMethodInterceptor
	{
		public object Invoke(AopAlliance.Intercept.IMethodInvocation invocation)
		{
			//object returnValue = "!";
			//return returnValue;

			object returnValue = invocation.Proceed();
			return returnValue;
		}
	}
}
