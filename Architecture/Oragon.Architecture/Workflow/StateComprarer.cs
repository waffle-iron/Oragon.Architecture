using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
	public class StateComprarer<StateType> : IEqualityComparer<State<StateType>>
	where StateType : IComparable
	{
		public bool Equals(State<StateType> x, State<StateType> y)
		{
			var defValue = default(State<StateType>);
			if (x == defValue && y == defValue)
				return true;
			else if (x != defValue && y != defValue)
				return x.StateValue.CompareTo(y.StateValue) == 0;
			return false;
		}

		public int GetHashCode(State<StateType> obj)
		{
			return obj.StateValue.GetHashCode();
		}
	}
}
