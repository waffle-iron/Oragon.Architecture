using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Oragon.Architecture.Workflow.Facility;
using System.Linq;
//using Oragon.Architecture.Workflow.QueuedWorkFlow;
using Oragon.Architecture.Workflow;
using Oragon.Architecture.Workflow.QueuedWorkFlow;
using System.Collections.Generic;

namespace Oragon.Architecture.Tests.Workflow
{
	[TestClass]
	public class WorkflowTest : TestBase
	{
		public Spring.Messaging.Amqp.Rabbit.Core.RabbitTemplate RabbitTemplate { get; set; }
		public QueuedStateMachine StateMachine { get; set; }

		[TestMethod]
		public void InitialUniqueTransitionTest()
		{
			var initialTransitions = this.StateMachine.GetInitialTransitions();
			Assert.AreEqual(initialTransitions.Count(), 1);
		}


		[TestMethod]
		public void TransitionsAreQueued()
		{
			var transitions = this.StateMachine.GetInitialTransitions();
			foreach (var transition in transitions)
				Assert.IsInstanceOfType(transition, typeof(QueuedTransition));
		}

		[TestMethod]
		public void FlowTest()
		{
			var initialTransition = this.StateMachine.GetInitialTransitions().Single();
			var possibleSteps = this.StateMachine.GetPossibleTransitions(initialTransition.Destination);
			Assert.AreEqual(possibleSteps.Count(), 1);
			var nextTransition = (QueuedTransition)possibleSteps.Single();
			Assert.AreEqual(nextTransition.QueueToListen, "ToTrack.Queue");
		}

		[TestMethod]
		public void TestePipe()
		{
			var messageToSend = new WorkflowTestMessage() { ID = 77, Messages = new List<string>() };
			string routingKey = this.StateMachine.GetInitialTransitions().Single().BuildRoutingKey();
			this.StateMachine.Start();
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			this.RabbitTemplate.ConvertAndSend(routingKey, messageToSend);
			System.Threading.Thread.Sleep(new TimeSpan(0, 0, 20));
			this.StateMachine.Stop();
		}

	}

	public class WorkflowTestHandler
	{
		public string Message { get; set; }

		public void Process(WorkflowTestMessage workflowTestMessage)
		{
			if (Message == "state:MetadadoArmazenado -> state:MetadadoIngerido")
				throw new Exception("Teste Exception");

			workflowTestMessage.Messages.Add(Message);
			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine("###################################");
			foreach (string texto in workflowTestMessage.Messages)
			{
				System.Diagnostics.Debug.WriteLine(texto);
			}
			System.Diagnostics.Debug.WriteLine("###################################");
		}
	}

	public class WorkflowTestMessage
	{
		public int ID { get; set; }
		public List<string> Messages { get; set; }
	}
}
