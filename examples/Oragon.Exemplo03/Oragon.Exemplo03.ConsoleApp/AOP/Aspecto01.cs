using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Exemplo03.ConsoleApp.AOP
{
	public class Aspecto01: AopAlliance.Intercept.IMethodInterceptor
	{
		public object Invoke(AopAlliance.Intercept.IMethodInvocation invocation)
		{
			object returnValue = null;
			string className = invocation.TargetType.Namespace + "." + invocation.TargetType.Name;
			string methodName = invocation.Method.Name;

			ConsoleColor oldColor = Console.ForegroundColor;
			///////////////////////////////////////////////////////////////
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Estou chamando uma instância do tipo {0}, chamando o método {1}", className, methodName);
			Console.ForegroundColor = oldColor;
			///////////////////////////////////////////////////////////////


			returnValue = invocation.Proceed();


			///////////////////////////////////////////////////////////////
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("A chamada foi concluída chamando uma instância do tipo {0}, chamando o método {1}", className, methodName);
			Console.ForegroundColor = oldColor;
			///////////////////////////////////////////////////////////////


			return returnValue;
		}
	}
}
