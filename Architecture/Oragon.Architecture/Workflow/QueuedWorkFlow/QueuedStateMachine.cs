using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Workflow.Facility;
using Spring.Messaging.Amqp.Core;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedStateMachine : StateMachine<QueuedTransition, StringState>, ISer
	{
		protected Spring.Messaging.Amqp.Rabbit.Connection.CachingConnectionFactory AdminAmqpConnection { get; set; }
		protected Spring.Messaging.Amqp.Rabbit.Connection.CachingConnectionFactory UserAmqpConnection { get; set; }
		//Spring.Messaging.Amqp.Rabbit.Admin.RabbitBrokerAdmin RabbitBrokerAdmin { get; set; }
		public IAmqpAdmin AmqpAdmin { get; set; }

		protected string AmqpQueuePrefix { get; set; }
		protected string AmqpExchangePrefix { get; set; }


		public void Initialize()
		{
			Func<string, string> getQueueName = (queueName => string.Format("{0}{1}", this.AmqpQueuePrefix, queueName));
			Func<string, string> getExchangeName = (exchangeName => string.Format("{0}{1}", this.AmqpExchangePrefix, exchangeName));


			foreach (QueuedTransition queuedTransition in this.Transitions)
			{
				IExchange exchange = new Spring.Messaging.Amqp.Core.DirectExchange(getExchangeName(queuedTransition.ExchangeName), true, false);
				this.AmqpAdmin.DeclareExchange(exchange);

				Queue errorQueue = new Queue(getQueueName(queuedTransition.QueueToReportError), true, false, false);
				this.AmqpAdmin.DeclareQueue(errorQueue);

				Dictionary<string, string> queueArgs = new Dictionary<string, string>();
				queueArgs.Add("x-dead-letter-exchange", exchange.Name);
				queueArgs.Add("x-dead-letter-routing-key", queuedTransition.BuildFailureRoutingKey());
				Queue processQueue = new Queue(getQueueName(queuedTransition.QueueToListen), true, false, false, queueArgs);
				this.AmqpAdmin.DeclareQueue(processQueue);


				Binding processBinding = new Binding(processQueue.Name, Binding.DestinationType.Queue, exchange.Name, queuedTransition.BuildRoutingKey(), null);
				this.AmqpAdmin.DeclareBinding(processBinding);

				Binding failureBinding = new Binding(errorQueue.Name, Binding.DestinationType.Queue, exchange.Name, queuedTransition.BuildFailureRoutingKey(), null);
				this.AmqpAdmin.DeclareBinding(failureBinding);
			}


		}

	}
}
