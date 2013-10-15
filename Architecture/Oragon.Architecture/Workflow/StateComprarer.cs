using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
    public class StateComprarer<StateType> : IEqualityComparer<StateType>
           where StateType : State
    {
        public bool Equals(StateType x, StateType y)
        {
            var defValue = default(State);
            if (x == defValue && y == defValue)
                return true;
            else if (x != defValue && y != defValue)
                return x.GetValue().CompareTo(y.GetValue()) == 0;
            return false;
        }

        public int GetHashCode(StateType obj)
        {
            return obj.GetValue().GetHashCode();
        }
    }
}
