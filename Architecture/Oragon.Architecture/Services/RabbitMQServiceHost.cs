using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Spring.Context;
using Spring.Messaging.Amqp.Rabbit.Core;
using Spring.Messaging.Amqp.Support.Converter;
using Spring.Objects.Factory;
using Spring.Util;

namespace Oragon.Architecture.Services
{
    public class RabbitMQServiceHost : IInitializingObject, IObjectNameAware, IService
    {
        public Spring.Messaging.Amqp.Rabbit.Connection.IConnectionFactory ConnectionFactory { get; set; }
        public int ConcurrentConsumers { get; set; }
        public object Service { get; set; }
        public bool AutoStart { get; set; }
        public Type ServiceInterface { get; set; }
        public bool DropQueueAfterLastListenerStop { get; set; }

        private List<Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer> messageListenerContainers;

        public void AfterPropertiesSet()
        {
            this.BuildMessageListenerContainers();
            this.messageListenerContainers.ForEach(it => it.AfterPropertiesSet());
            if (this.AutoStart)
                this.Start();
        }


        private void BuildMessageListenerContainers()
        {
            Spring.Messaging.Amqp.Core.IAmqpAdmin rabbitAdmin = new Spring.Messaging.Amqp.Rabbit.Core.RabbitAdmin(this.ConnectionFactory);
            this.messageListenerContainers = new List<Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer>();

            RabbitTemplate template = this.BuildRabbitTemplate();
            JsonMessageConverter messageConverter = new JsonMessageConverter();

            foreach (MethodInfo methodInfo in this.ServiceInterface.GetMethods())
            {
                string queueName = RabbitMQServiceUtils.GetQueueName(methodInfo);
                Spring.Messaging.Amqp.Core.Queue processQueue = new Spring.Messaging.Amqp.Core.Queue(queueName, true, false, this.DropQueueAfterLastListenerStop);
                rabbitAdmin.DeclareQueue(processQueue);

                var messageListenerContainer = new Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer(this.ConnectionFactory);
                messageListenerContainer.AutoStartup = false;
                messageListenerContainer.ConcurrentConsumers = this.ConcurrentConsumers;
                messageListenerContainer.QueueNames = new string[] { queueName };
                messageListenerContainers.Add(messageListenerContainer);

                Action<Spring.Messaging.Amqp.Core.Message> messageListener = (requestMessage =>
                 {
                     MessageEnvelope requestEnvelope = (MessageEnvelope)messageConverter.FromMessage(requestMessage);

                     Spring.Objects.Support.MethodInvoker methodInvoker = new Spring.Objects.Support.MethodInvoker();
                     foreach (var item in requestEnvelope.Arguments)
                         methodInvoker.AddNamedArgument(item.Key, item.Value);
                     methodInvoker.TargetObject = this.Service;
                     methodInvoker.TargetMethod = methodInfo.Name;
                     methodInvoker.Prepare();
                     MessageEnvelope responseEnvelope = new MessageEnvelope();
                     try
                     {
                         responseEnvelope.ReturnValue = methodInvoker.Invoke();
                         if (responseEnvelope.ReturnValue == System.Reflection.Missing.Value)
                             responseEnvelope.ReturnValue = null;
                     }
                     catch (System.Reflection.TargetInvocationException exception)
                     {
                         responseEnvelope.Exception = exception.InnerException;
                     }
                     catch (Exception exception)
                     {
                         responseEnvelope.Exception = exception;
                     }
                     Spring.Messaging.Amqp.Core.Address replyTo = requestMessage.MessageProperties.ReplyToAddress;
                     Spring.Messaging.Amqp.Core.Message responseMessage = messageConverter.ToMessage(responseEnvelope, new Spring.Messaging.Amqp.Core.MessageProperties());
                     template.Send(replyTo.ExchangeName, replyTo.RoutingKey, responseMessage);
                 });
                messageListenerContainer.MessageListener = messageListener;
            }
        }

        private Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate BuildRabbitTemplate()
        {
            Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate template = new Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate(this.ConnectionFactory);
            template.ChannelTransacted = true;
            template.ReplyTimeout = 4000;
            template.Immediate = true;
            template.MessageConverter = new Spring.Messaging.Amqp.Support.Converter.JsonMessageConverter()
            {
                CreateMessageIds = false
            };
            return template;
        }




        public bool IsRunning
        {
            get { return this.messageListenerContainers.Any(it => it.IsRunning); }
        }

        public void Start()
        {
            this.messageListenerContainers.ForEach(it => it.Start());
        }

        public void Stop()
        {
            this.messageListenerContainers.ForEach(it => it.Stop());
        }

        public string ObjectName { get; set; }

        public string Name
        {
            get { return "RabbitMQServiceHost"; }
        }
    }
}
