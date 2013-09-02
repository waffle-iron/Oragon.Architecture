using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Exemplo03.ConsoleApp.AOP
{
	public class Aspecto02 : AopAlliance.Intercept.IMethodInterceptor
	{
		public Exception ExcetionParaSerRelancada { get; set; }

		public object Invoke(AopAlliance.Intercept.IMethodInvocation invocation)
		{
			object returnValue = null;
			try
			{
				returnValue = invocation.Proceed();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Uma exception foi lançada -> " + ex.ToString());
				//Usar Mecanismo de Log para armazenar a exception	
				throw ExcetionParaSerRelancada;
			}
			return returnValue;
		}
	}
}
