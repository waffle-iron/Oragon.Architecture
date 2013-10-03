using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
	public class TransitionComprarer<StateType> : IEqualityComparer<Transition<StateType>>
		where StateType : IComparable
	{
		private StateComprarer<StateType> StateComprarer;

		public TransitionComprarer()
		{
			this.StateComprarer = new StateComprarer<StateType>();
		}


		public bool Equals(Transition<StateType> x, Transition<StateType> y)
		{
			var defValue = default(Transition<StateType>);
			if (x == defValue && y == defValue)
				return true;
			else if (x != defValue && y != defValue)
				return (
					this.StateComprarer.Equals(x.Origin, y.Origin)
					&&
					this.StateComprarer.Equals(x.Destination, y.Destination)
				);
			return false;
		}

		public int GetHashCode(Transition<StateType> obj)
		{
			return string.Concat(obj.Origin, "|", obj.Destination).GetHashCode();
		}
	}
}
