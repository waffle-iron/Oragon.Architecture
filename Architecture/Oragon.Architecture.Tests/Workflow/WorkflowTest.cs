using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oragon.Architecture.Workflow.Facility;
using System.Linq;
using Oragon.Architecture.Workflow.QueuedWorkFlow;
using Oragon.Architecture.Workflow;

namespace Oragon.Architecture.Tests.Workflow
{
	[TestClass]
	public class WorkflowTest : TestBase
	{

		public Oragon.Architecture.Tests.Workflow.QueuedStateMachine StateMachine { get; set; }

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
			var possibleSteps = this.StateMachine.GetPossibleTransitions(initialTransition.Destination.StateValue);
		 	Assert.AreEqual(possibleSteps.Count(), 1);
			var nextTransition = (QueuedTransition)possibleSteps.Single();
			Assert.AreEqual(nextTransition.QueueToListen, "ToTrack.Queue");
		}

	}

	public class QueuedStateMachine : QueuedStateMachine<string>
	{
	}




}
