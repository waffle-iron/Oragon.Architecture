using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
	public class State<StateType>
	where StateType : IComparable
	{
		public StateType StateValue { get; set; }
		public string StateName { get; set; }
		public Dictionary<string, object> ExtendedProperties { get; private set; }

		public State()
		{
			this.ExtendedProperties = new Dictionary<string, object>();
		}
	}

}
