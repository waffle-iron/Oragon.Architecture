using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Spring.Context;
using Spring.Objects.Factory;
using Spring.Util;

namespace Oragon.Architecture.Services
{
	public class RabbitMQServiceHost : IInitializingObject, ILifecycle, IObjectNameAware
	{
		protected Spring.Messaging.Amqp.Rabbit.Connection.IConnectionFactory AmqpConnection { get; set; }
		protected int ConcurrentConsumers { get; set; }
		protected object Service { get; set; }
		protected Type ServiceInterface { get; set; }
		public Spring.Messaging.Amqp.Core.IAmqpAdmin AmqpAdmin { get; set; }

		private List<Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer> messageListenerContainers;

		public void AfterPropertiesSet()
		{
			this.BuildMessageListenerContainers();
			this.messageListenerContainers.ForEach(it => it.AfterPropertiesSet());
			this.Start();
		}


		private void BuildMessageListenerContainers()
		{
			this.messageListenerContainers = new List<Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer>();

			foreach (MethodInfo methodInfo in this.ServiceInterface.GetMethods())
			{
				string queueName = RabbitMQServiceUtils.GetQueueName(methodInfo);
				var processQueue = new Spring.Messaging.Amqp.Core.Queue(queueName, true, false, false);
				this.AmqpAdmin.DeclareQueue(processQueue);

				var messageListenerAdapter = new Spring.Messaging.Amqp.Rabbit.Listener.Adapter.MessageListenerAdapter();
				messageListenerAdapter.MessageConverter = new Spring.Messaging.Amqp.Support.Converter.JsonMessageConverter();
				messageListenerAdapter.DefaultListenerMethod = methodInfo.Name;
				messageListenerAdapter.HandlerObject = this.Service;

				var messageListenerContainer = new Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer(this.AmqpConnection);
				messageListenerContainer.AutoStartup = false;
				messageListenerContainer.ConcurrentConsumers = this.ConcurrentConsumers;
				messageListenerContainer.QueueNames = new string[] { queueName };
				messageListenerContainer.MessageListener = messageListenerAdapter;
				messageListenerContainers.Add(messageListenerContainer);
			}
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
	}
}
