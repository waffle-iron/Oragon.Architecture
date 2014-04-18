using System;
using System.Collections.Generic;
using System.Linq;
using Spring.Messaging.Amqp.Core;
using Spring.Objects.Factory;
using Oragon.Architecture.Services;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedStateMachine : StateMachine<QueuedTransition, string>, IInitializingObject, IObjectNameAware, IService
	{
		protected Spring.Messaging.Amqp.Rabbit.Connection.IConnectionFactory UserAmqpConnection { get; set; }
		//Spring.Messaging.Amqp.Rabbit.Admin.RabbitBrokerAdmin RabbitBrokerAdmin { get; set; }
		public IAmqpAdmin AmqpAdmin { get; set; }

		protected string AmqpQueuePrefix { get; set; }
		protected string AmqpExchangePrefix { get; set; }
		public bool CreateZombieQueues { get; set; }



		private List<Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer> MessageContainers;

		public QueuedStateMachine()
		{
			this.MessageContainers = new List<Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer>();
		}

		private void InitializeBroker()
		{
			foreach (QueuedTransition queuedTransition in this.Transitions)
			{
				this.ConfigureBroker(queuedTransition);
			}
		}

		private void InitializeServices()
		{
			foreach (QueuedTransition queuedTransition in this.Transitions)
			{
				Spring.Objects.Support.MethodInvoker methodInvoker = new Spring.Objects.Support.MethodInvoker();
				methodInvoker.TargetObject = queuedTransition.Service;
				methodInvoker.TargetMethod = "Initialize";
				methodInvoker.Prepare();
				if (methodInvoker.GetPreparedMethod() != null)
				{
					methodInvoker.Invoke();
				}
			}
		}

		private void ConfigureBroker(QueuedTransition queuedTransition)
		{
			QueuedTransition nextQueuedTransition = this.GetPossibleTransitions(queuedTransition.Destination).FirstOrDefault();
			//Listener
			Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer container = new Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer(this.UserAmqpConnection);
			container.DefaultRequeueRejected = true;
			container.ObjectName = this.ObjectName;
			container.AutoStartup = false;
			container.ConcurrentConsumers = queuedTransition.ConcurrentConsumers;
			QueuedWorkflowMessageListenerAdapter messageListenerAdapter = new QueuedWorkflowMessageListenerAdapter()
			{
				AmqpAdmin = this.AmqpAdmin,
				AmqpQueuePrefix = this.AmqpQueuePrefix,
				AmqpExchangePrefix = this.AmqpExchangePrefix,
				MessageConverter = new Spring.Messaging.Amqp.Support.Converter.JsonMessageConverter(),
				DefaultListenerMethod = queuedTransition.ServiceMethod,
				HandlerObject = queuedTransition.Service
			};
			messageListenerAdapter.Configure(
				queuedTransition,
				nextQueuedTransition,
				this.CreateZombieQueues
			);
			container.MessageListener = messageListenerAdapter;
			container.QueueNames = new string[] { messageListenerAdapter.ReceiveQueue.Name };
			this.MessageContainers.Add(container);
		}

		public void AfterPropertiesSet()
		{
			this.InitializeBroker();
			this.MessageContainers.ForEach(it => it.AfterPropertiesSet());
			this.InitializeServices();
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

		public string Name
		{
			get { return "QueuedStateMachine"; }
		}
	}
}
