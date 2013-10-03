using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
	public class Transition<StateType>
	where StateType : IComparable
	{
		public State<StateType> Origin { get; set; }
		public State<StateType> Destination { get; set; }
		public Dictionary<string, object> ExtendedProperties { get; private set; }
	}

}

