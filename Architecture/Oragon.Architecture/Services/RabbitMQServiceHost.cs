using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Objects.Factory;

namespace Oragon.Architecture.Services
{
	public class RabbitMQServiceHost : IInitializingObject, ILifecycle, IObjectNameAware
	{
		protected Spring.Messaging.Amqp.Rabbit.Connection.IConnectionFactory AmqpConnection { get; set; }
		protected int ConcurrentConsumers { get; set; }
		protected object Service { get; set; }
		protected string ServiceMethod { get; set; }
		protected string QueueName { get; set; }
		public Spring.Messaging.Amqp.Core.IAmqpAdmin AmqpAdmin { get; set; }

		private Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer messageListenerContainer;
		private Spring.Messaging.Amqp.Rabbit.Listener.Adapter.MessageListenerAdapter messageListenerAdapter;

		public void AfterPropertiesSet()
		{
			this.BuildMessageListenerAdapter();
			this.BuildMessageListenerContainer();
			messageListenerContainer.AfterPropertiesSet();
			this.Start();
		}

		private void BuildMessageListenerContainer()
		{
			this.messageListenerContainer = new Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer(this.AmqpConnection);
			this.messageListenerContainer.AutoStartup = false;
			this.messageListenerContainer.ConcurrentConsumers = this.ConcurrentConsumers;
			this.messageListenerContainer.QueueNames = new string[] { this.QueueName };
			this.messageListenerContainer.MessageListener = messageListenerAdapter;

			var processQueue = new Spring.Messaging.Amqp.Core.Queue(this.messageListenerContainer.QueueNames.Single(), true, false, false);
			this.AmqpAdmin.DeclareQueue(processQueue);
		}

		private void BuildMessageListenerAdapter()
		{
			this.messageListenerAdapter = new Spring.Messaging.Amqp.Rabbit.Listener.Adapter.MessageListenerAdapter();
			this.messageListenerAdapter.MessageConverter = new Spring.Messaging.Amqp.Support.Converter.JsonMessageConverter();
			this.messageListenerAdapter.DefaultListenerMethod = this.ServiceMethod;
			this.messageListenerAdapter.HandlerObject = this.Service;
		}


		public bool IsRunning
		{
			get { return this.messageListenerContainer.IsRunning; }
		}

		public void Start()
		{
			this.messageListenerContainer.Start();
		}

		public void Stop()
		{
			this.messageListenerContainer.Stop();
		}

		public string ObjectName { get; set; }
	}
}
