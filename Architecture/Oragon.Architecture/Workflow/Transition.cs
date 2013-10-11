using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
	public abstract class Transition<StateType>
	where StateType : IComparable
	{
		public State<StateType> Origin { get; set; }
		public State<StateType> Destination { get; set; }
	}

}

