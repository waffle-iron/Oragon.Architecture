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
        public IConnectionFactory ConnectionFactory { get; set; }
        public long Timeout { get; set; }
        public Type ServiceInterface { get; set; }
        private Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate rabbitTemplate;
        private object Proxy { get; set; }

        public void AfterPropertiesSet()
        {
            this.Proxy = this.CreateProxy();
            this.rabbitTemplate = this.BuildRabbitTemplate();
            this.rabbitTemplate.AfterPropertiesSet();
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
            MessageEnvelope requestMessage = BuildMessage(invocation);
            string queueName = RabbitMQServiceUtils.GetQueueName(invocation.Method);
            MessageEnvelope responseMessage = (MessageEnvelope)this.rabbitTemplate.ConvertSendAndReceive(queueName, requestMessage);
            if (responseMessage.Exception != null)
            {
                Exception exceptionToRethrow = JsonHelper.ReconstructException(responseMessage.Exception);
                throw exceptionToRethrow;
            }
            return responseMessage.ReturnValue;
        }

        private static MessageEnvelope BuildMessage(IMethodInvocation invocation)
        {
            MessageEnvelope requestMessage = new MessageEnvelope();
            requestMessage.Arguments = new Dictionary<string, object>();
            requestMessage.ReturnValue = null;
            requestMessage.Exception = null;
            string[] parameterNames = invocation.Method.GetParameters().Select(it => it.Name).ToArray();
            for (int i = 0; i < parameterNames.Length; i++)
            {
                requestMessage.Arguments.Add(parameterNames[i], invocation.Arguments[i]);
            }
            return requestMessage;
        }
    }
}
