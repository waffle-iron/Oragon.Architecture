using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AopAlliance.Intercept;
using Spring.Aop.Framework;
using Spring.Messaging.Amqp.Rabbit.Connection;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Config;
using Spring.Proxy;
using Spring.Util;

namespace Oragon.Architecture.Services
{
	public class RabbitMQServiceClientFactory : IFactoryObject, IInitializingObject, IConfigurableFactoryObject, IMethodInterceptor
	{
		protected IConnectionFactory ConnectionFactory { get; set; }
		protected long Timeout { get; set; }
		protected Type ServiceInterface { get; set; }
		private Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate rabbitTemplate;
		private object Proxy { get; set; }

		public void AfterPropertiesSet()
		{
			this.Proxy = this.CreateProxy();
			this.rabbitTemplate = this.BuildRabbitTemplate();
			this.ValidateConfiguration();
		}

		private object CreateProxy()
		{
			ProxyFactory proxy = new ProxyFactory(this.ServiceInterface, this);
			return proxy.GetProxy();
		}

		private void ValidateConfiguration()
		{
			if (this.ServiceInterface == null)
				throw new ArgumentNullException("ServiceInterface nula não é permitída");
			if (this.ServiceInterface.IsInterface == false)
				throw new InvalidCastException("ServiceInterface informado não é uma interface");
		}

		private Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate BuildRabbitTemplate()
		{
			Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate template = new Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate(this.ConnectionFactory);
			template.ChannelTransacted = true;
			template.ReplyTimeout = this.Timeout;
			template.Immediate = true;
			template.MessageConverter = new Spring.Messaging.Amqp.Support.Converter.JsonMessageConverter()
			{
				CreateMessageIds = true
			};
			template.AfterPropertiesSet();
			return template;
		}

		public object GetObject()
		{
			return this.Proxy;
		}

		public Type ObjectType { get { return this.ServiceInterface; } }

		public bool IsSingleton { get { return true; } }

		public IObjectDefinition ProductTemplate { get; set; }

		public object Invoke(IMethodInvocation invocation)
		{
			string queueName = RabbitMQServiceUtils.GetQueueName(invocation.Method);
			object returnValue = null;
			if (invocation.Method.ReturnType == typeof(void))
			{
				this.rabbitTemplate.ConvertAndSend(queueName, invocation.Arguments.First());
			}
			else
			{
				returnValue = this.rabbitTemplate.ConvertSendAndReceive(queueName, invocation.Arguments.First());
			}
			return returnValue;
		}
	}
}
