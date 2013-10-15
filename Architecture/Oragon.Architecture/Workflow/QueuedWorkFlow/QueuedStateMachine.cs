using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Workflow.Facility;
using Spring.Messaging.Amqp.Core;
using Oragon.Architecture.Extensions;
using Spring.Objects.Factory;
using Spring.Context;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedStateMachine : StateMachine<QueuedTransition, string>, IInitializingObject, ILifecycle, IObjectNameAware
	{
		protected Spring.Messaging.Amqp.Rabbit.Connection.CachingConnectionFactory UserAmqpConnection { get; set; }
		//Spring.Messaging.Amqp.Rabbit.Admin.RabbitBrokerAdmin RabbitBrokerAdmin { get; set; }
		public IAmqpAdmin AmqpAdmin { get; set; }

		protected string AmqpQueuePrefix { get; set; }
		protected string AmqpExchangePrefix { get; set; }

		Func<string, string> getQueueName;
		Func<string, string> getExchangeName;

		private List<Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer> MessageContainers;

		private void InitializeAmqpBroker()
		{
			foreach (QueuedTransition queuedTransition in this.Transitions)
			{
				queuedTransition.Exchange = new Spring.Messaging.Amqp.Core.DirectExchange(getExchangeName(queuedTransition.ExchangeName), true, false);
				this.AmqpAdmin.DeclareExchange(queuedTransition.Exchange);

				queuedTransition.ProcessQueue = new Queue(this.getQueueName(queuedTransition.QueueToListen), true, false, false);
				this.AmqpAdmin.DeclareQueue(queuedTransition.ProcessQueue);

				queuedTransition.FailureQueue = new Queue(this.getQueueName(queuedTransition.QueueToReportError), true, false, false);
				this.AmqpAdmin.DeclareQueue(queuedTransition.FailureQueue);

				queuedTransition.ProcessBinding = new Binding(queuedTransition.ProcessQueue.Name, Binding.DestinationType.Queue, queuedTransition.Exchange.Name, queuedTransition.BuildRoutingKey(), null);
				this.AmqpAdmin.DeclareBinding(queuedTransition.ProcessBinding);

				queuedTransition.FailureBinding = new Binding(queuedTransition.FailureQueue.Name, Binding.DestinationType.Queue, queuedTransition.Exchange.Name, queuedTransition.BuildFailureRoutingKey(), null);
				this.AmqpAdmin.DeclareBinding(queuedTransition.FailureBinding);
			}
		}

		private void InitializePipeline()
		{
			this.MessageContainers = new List<Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer>();

			foreach (QueuedTransition queuedTransition in this.Transitions)
			{
				Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer container = new Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer(this.UserAmqpConnection);
				container.ObjectName = this.ObjectName;
				container.AutoStartup = false;
				container.QueueNames = new string[] { this.getQueueName(queuedTransition.QueueToListen) };
				container.ConcurrentConsumers = queuedTransition.ConcurrentConsumers;
				QueuedWorkflowMessageListenerAdapter messageListenerAdapter = new QueuedWorkflowMessageListenerAdapter();
				messageListenerAdapter.MessageConverter = new Spring.Messaging.Amqp.Support.Converter.JsonMessageConverter();
				messageListenerAdapter.DefaultListenerMethod = queuedTransition.ServiceMethod;
				messageListenerAdapter.HandlerObject = queuedTransition.Service;

				messageListenerAdapter.ResponseFailureExchange = this.getExchangeName(queuedTransition.ExchangeName);
				messageListenerAdapter.ResponseFailureRoutingKey = queuedTransition.BuildFailureRoutingKey();

				QueuedTransition nextQueuedTransition = this.GetPossibleTransitions(queuedTransition.Destination).FirstOrDefault();
				if (nextQueuedTransition != null)
				{
					messageListenerAdapter.ResponseExchange = this.getExchangeName(nextQueuedTransition.ExchangeName);
					messageListenerAdapter.ResponseRoutingKey = nextQueuedTransition.BuildRoutingKey();
				}
				container.MessageListener = messageListenerAdapter;
				this.MessageContainers.Add(container);
			}

		}

		public void AfterPropertiesSet()
		{
			this.getQueueName = (queueName => string.Format("{0}{1}", this.AmqpQueuePrefix, queueName));
			this.getExchangeName = (exchangeName => string.Format("{0}{1}", this.AmqpExchangePrefix, exchangeName));
			this.InitializeAmqpBroker();
			this.InitializePipeline();
			this.MessageContainers.ForEach(it => it.AfterPropertiesSet());
		}

		public bool IsRunning
		{
			get { return this.MessageContainers.Any(it => it.IsRunning); }
		}

		public void Start()
		{
			this.MessageContainers.ForEach(it => it.Start());
		}

		public void Stop()
		{
			this.MessageContainers.ForEach(it => it.Stop());
		}

		public string ObjectName { get; set; }
	}
}
