using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Workflow.Facility;
using Spring.Messaging.Amqp.Core;
using Oragon.Architecture.Extensions;
using Spring.Objects.Factory;
using Spring.Context;
using Oragon.Architecture.Services;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedStateMachine : StateMachine<QueuedTransition, string>, IInitializingObject, ILifecycle, IObjectNameAware, IService
	{
		protected Spring.Messaging.Amqp.Rabbit.Connection.IConnectionFactory UserAmqpConnection { get; set; }
		//Spring.Messaging.Amqp.Rabbit.Admin.RabbitBrokerAdmin RabbitBrokerAdmin { get; set; }
		public IAmqpAdmin AmqpAdmin { get; set; }

		protected string AmqpQueuePrefix { get; set; }
		protected string AmqpExchangePrefix { get; set; }
		public bool CreateZombieQueues { get; set; }


		Func<string, string> getQueueName;
		Func<string, string> getExchangeName;

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

		private void ConfigureBroker(QueuedTransition queuedTransition)
		{
			//Listener
			Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer container = new Spring.Messaging.Amqp.Rabbit.Listener.SimpleMessageListenerContainer(this.UserAmqpConnection);
			container.ObjectName = this.ObjectName;
			container.AutoStartup = false;
			container.ConcurrentConsumers = queuedTransition.ConcurrentConsumers;
			QueuedWorkflowMessageListenerAdapter messageListenerAdapter = new QueuedWorkflowMessageListenerAdapter();
			messageListenerAdapter.MessageConverter = new Spring.Messaging.Amqp.Support.Converter.JsonMessageConverter();
			messageListenerAdapter.DefaultListenerMethod = queuedTransition.ServiceMethod;
			messageListenerAdapter.HandlerObject = queuedTransition.Service;
			container.MessageListener = messageListenerAdapter;


			//Exchange de recebimento de mensagens
			var exchange = new Spring.Messaging.Amqp.Core.TopicExchange(getExchangeName(queuedTransition.ExchangeName), true, false);
			this.AmqpAdmin.DeclareExchange(exchange);

			//Queue de processamento da Mensagem
			var processQueue = new Queue(this.getQueueName(queuedTransition.LogicalQueueName) + ".Process", true, false, false);
			this.AmqpAdmin.DeclareQueue(processQueue);

			//Binding de processamento das Mensagens
			var processBinding = new Binding(processQueue.Name, Binding.DestinationType.Queue, exchange.Name, queuedTransition.BuildRoutingKey(), null);
			this.AmqpAdmin.DeclareBinding(processBinding);
			container.QueueNames = new string[] { processQueue.Name };


			if (this.CreateZombieQueues)
			{
				//Ao criar uma fila Zumbi, não é necessário realizar operação alguma, apenas configurar o binding. As mensagens na fila zumbi são acumuladas sem consumo.

				//Zombie
				var zombieQueue = new Queue(this.getQueueName(queuedTransition.LogicalQueueName) + ".Zombie", true, false, false);
				this.AmqpAdmin.DeclareQueue(zombieQueue);

				var zombieBinding = new Binding(zombieQueue.Name, Binding.DestinationType.Queue, exchange.Name, queuedTransition.BuildRoutingKey(), null);
				this.AmqpAdmin.DeclareBinding(zombieBinding);
			}

			//Configuração para Passos de Sucesso
			QueuedTransition nextQueuedTransition = this.GetPossibleTransitions(queuedTransition.Destination).FirstOrDefault();
			if (nextQueuedTransition != null)
			{
				messageListenerAdapter.ResponseExchange = this.getExchangeName(nextQueuedTransition.ExchangeName);
				messageListenerAdapter.ResponseRoutingKey = nextQueuedTransition.BuildRoutingKey();
			}


			if (queuedTransition.Strategy == ExceptionStrategy.SendToErrorQueue)
			{
				//Failure
				var failureQueue = new Queue(this.getQueueName(queuedTransition.LogicalQueueName) + ".Failure", true, false, false);
				this.AmqpAdmin.DeclareQueue(failureQueue);

				var failureBinding = new Binding(failureQueue.Name, Binding.DestinationType.Queue, exchange.Name, queuedTransition.BuildFailureRoutingKey(), null);
				this.AmqpAdmin.DeclareBinding(failureBinding);

				messageListenerAdapter.ResponseFailureExchange = exchange.Name;
				messageListenerAdapter.ResponseFailureRoutingKey = failureBinding.RoutingKey;
			}
			else if (queuedTransition.Strategy == ExceptionStrategy.Requeue)
			{
				messageListenerAdapter.ResponseFailureExchange = exchange.Name;
				messageListenerAdapter.ResponseFailureRoutingKey = processBinding.RoutingKey;
			}
			else if (queuedTransition.Strategy == ExceptionStrategy.SendToNextStepQueue)
			{
				messageListenerAdapter.ResponseFailureExchange = messageListenerAdapter.ResponseExchange;
				messageListenerAdapter.ResponseFailureRoutingKey = messageListenerAdapter.ResponseRoutingKey;
			}
			this.MessageContainers.Add(container);
		}

		public void AfterPropertiesSet()
		{
			this.getQueueName = (queueName => string.Format("{0}{1}", this.AmqpQueuePrefix, queueName));
			this.getExchangeName = (exchangeName => string.Format("{0}{1}", this.AmqpExchangePrefix, exchangeName));
			this.InitializeBroker();
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

		public string Name
		{
			get { return "QueuedStateMachine"; }
		}
	}
}
