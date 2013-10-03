using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
	public class StateMachine<StateType>
	where StateType : IComparable
	{
		private StateComprarer<StateType> StateComprarer;
		private TransitionComprarer<StateType> TransitionComprarer;
		public List<Transition<StateType>> Transitions { get; set; }

		public StateMachine()
		{
			this.StateComprarer = new StateComprarer<StateType>();
			this.TransitionComprarer = new TransitionComprarer<StateType>();
		}

		#region Queries

		public IEnumerable<State<StateType>> GetAllStates()
		{
			return
				this.Transitions.Select(it => it.Origin)
				.Union(this.Transitions.Select(it => it.Destination))
				.Distinct(this.StateComprarer);
		}

		public IEnumerable<Transition<StateType>> GetPossibleTransitions(StateType stateValue)
		{
			return this.Transitions.Where(it => Comparer<StateType>.Equals(it.Origin.StateValue, stateValue));
		}

		public IEnumerable<State<StateType>> GetPossibleDestinations(StateType stateValue)
		{
			return this.GetPossibleTransitions(stateValue).Select(it => it.Destination);
		}

		public Transition<StateType> GetTransition(StateType sourceValue, StateType targetValue)
		{
			return this.Transitions.Where(it =>
						it.Origin.Equals(sourceValue)
						&&
						it.Destination.Equals(targetValue)
			).FirstOrDefault();
		}

		public IEnumerable<State<StateType>> GetFinalStates()
		{
			IEnumerable<State<StateType>> returnValue = this.GetFinalTransitions()
										.Select(it => it.Destination)
										.Distinct(this.StateComprarer);
			return returnValue;
		}

		public IEnumerable<Transition<StateType>> GetFinalTransitions()
		{
			IEnumerable<Transition<StateType>> returnValue = this.Transitions.Where(it =>
													this.Transitions.Any(it2 => it2.Origin.Equals(it.Destination)) == false
										)
										.Distinct(this.TransitionComprarer);
			return returnValue;
		}

		public IEnumerable<State<StateType>> GetInitialStates()
		{
			IEnumerable<State<StateType>> returnValue = this.GetInitialTransitions()
										.Select(it => it.Origin)
										.Distinct(this.StateComprarer);
			return returnValue;
		}

		public IEnumerable<Transition<StateType>> GetInitialTransitions()
		{
			IEnumerable<Transition<StateType>> returnValue = this.Transitions.Where(it =>
													this.Transitions.Any(it2 => it2.Destination.Equals(it.Origin)) == false
										)
										.Distinct(this.TransitionComprarer);
			return returnValue;
		}
		#endregion
	}
}