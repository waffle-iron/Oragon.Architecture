using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
    public class TransitionComprarer<TransitionType, StateType> : IEqualityComparer<TransitionType>
        where StateType : State
        where TransitionType : Transition<StateType>
    {
        private StateComprarer<StateType> StateComprarer;

        public TransitionComprarer()
        {
            this.StateComprarer = new StateComprarer<StateType>();
        }


        public bool Equals(TransitionType x, TransitionType y)
        {
            var defValue = default(TransitionType);
            if (x == defValue && y == defValue)
                return true;
            else if (x != defValue && y != defValue)
                return (
                    this.StateComprarer.Equals(x.GetOrigin(), y.GetOrigin())
                    &&
                    this.StateComprarer.Equals(x.GetDestination(), y.GetDestination())
                );
            return false;
        }

        public int GetHashCode(TransitionType obj)
        {
            return string.Concat(obj.GetOrigin(), "|", obj.GetDestination()).GetHashCode();
        }
    }
}
