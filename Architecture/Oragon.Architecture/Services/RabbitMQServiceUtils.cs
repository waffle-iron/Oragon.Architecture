using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Oragon.Architecture.Services
{
	public class RabbitMQServiceUtils
	{
		public static string GetQueueName(MethodInfo methodInfo)
		{
			Type declaringType = methodInfo.DeclaringType;
			Type returnType = methodInfo.ReturnType;

			string inboundArgumentsText = string.Join(", ", methodInfo.GetParameters().Select(it => it.ParameterType.Namespace + "." + it.ParameterType.Name).ToArray());
			string outboundArgumentsText = string.Format("{0}.{1}", returnType.Namespace, returnType.Name);

			string returnValue = string.Format("{0}.{1}.{2}({3}):{4}",
				declaringType.Namespace, //0
				declaringType.Name, //1
				methodInfo.Name, //2
				inboundArgumentsText,//3
				outboundArgumentsText //4
				);

			return returnValue;
		}
	}
}
