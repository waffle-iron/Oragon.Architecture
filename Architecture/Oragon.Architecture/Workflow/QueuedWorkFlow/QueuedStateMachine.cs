using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedStateMachine<StateType> : StateMachine<StateType>
	where StateType : IComparable
	{
		protected Spring.Messaging.Amqp.Rabbit.Connection.CachingConnectionFactory AdminAmqpConnection { get; set; }
		protected Spring.Messaging.Amqp.Rabbit.Connection.CachingConnectionFactory UserAmqpConnection { get; set; }
		protected string AmqpItemPrefix { get; set; }

		public void Initialize()
		{




		}

	}
}
