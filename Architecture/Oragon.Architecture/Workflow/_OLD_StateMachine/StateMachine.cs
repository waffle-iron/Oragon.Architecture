//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Oragon.Architecture.StateMachine
//{
//	/// <summary>
//	/// Representa uma máquina de estados.
//	/// </summary>
//	/// <typeparam name="T">Tipo do estado.</typeparam>
//	public class StateMachine<StateType> 
//	{
//		public List<StateType> States
//		{
//			get
//			{
//				return this.Transitions.Select(it => it.Origin)
//				.Union(this.Transitions.Select(it => it.Destination))
//				.Distinct().ToList();
//			}
//		}
		
//		public List<Transition<StateType>> Transitions { get; set; }

//		public List<StateType> GetPossiblesDestinations(StateType stateValue)
//		{
//			return this.GetPossiblesTransitions(stateValue).Select(it => it.Destination).ToList();
//		}

//		public List<Transition<StateType>> GetPossiblesTransitions(StateType stateValue)
//		{
//			return this.Transitions.Where(it => it.Origin.Equals(stateValue)).ToList();
//		}

//		public Transition<StateType> GetTransition(StateType sourceValue, StateType targetValue)
//		{
//			return this.Transitions.Where(it => 
//						it.Origin.Equals(sourceValue) 
//						&& 
//						it.Destination.Equals(targetValue)
//			).FirstOrDefault();
//		}

//		public List<StateType> GetFinalStates()
//		{
//			List<StateType> returnValue = this.GetFinalTransitions()
//										.Select(it => it.Destination)
//										.Distinct()
//										.ToList();
//			return returnValue;
//		}

//		public List<Transition<StateType>> GetFinalTransitions()
//		{
//			List<Transition<StateType>> returnValue = this.Transitions.Where(it =>
//													this.Transitions.Any(it2 => it2.Origin.Equals(it.Destination)) == false
//										)
//										.Distinct()
//										.ToList();
//			return returnValue;
//		}

//		public List<StateType> GetInitialStates()
//		{
//			List<StateType> returnValue = this.GetInitialTransitions()
//										.Select(it => it.Origin)
//										.Distinct()
//										.ToList();
//			return returnValue;
//		}

//		public List<Transition<StateType>> GetInitialTransitions()
//		{
//			List<Transition<StateType>> returnValue = this.Transitions.Where(it =>
//													this.Transitions.Any(it2 => it2.Destination.Equals(it.Origin)) == false
//										)
//										.Distinct()
//										.ToList();
//			return returnValue;
//		}

//	}
//}
