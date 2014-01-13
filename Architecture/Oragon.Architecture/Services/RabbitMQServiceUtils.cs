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

		public static Spring.Messaging.Amqp.Support.Converter.IMessageConverter GetMessageConverter()
		{
			var messageConverter = new Spring.Messaging.Amqp.Support.Converter.JsonMessageConverter()
			{
				CreateMessageIds = true,
			};
			messageConverter.JsonSerializer = new Newtonsoft.Json.JsonSerializer()
			{
				MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
				TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
				Formatting = Newtonsoft.Json.Formatting.Indented,
				MaxDepth = null,
				PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All,
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
				ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Auto,
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
			};

			return messageConverter;
		}

		public static Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate BuildRabbitTemplate(Spring.Messaging.Amqp.Rabbit.Connection.IConnectionFactory connectionFactory, long timeout)
		{
			Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate template = new Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate(connectionFactory);
			template.ChannelTransacted = true;
			template.ReplyTimeout = timeout;
			template.Immediate = true;
			template.MessageConverter = RabbitMQServiceUtils.GetMessageConverter();
			return template;
		}


	}
}
