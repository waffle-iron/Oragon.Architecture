using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow
{
    public abstract class Transition
    {
        //public abstract State GetOrigin();
        //public abstract State GetDestination();
    }

    public abstract class Transition<StateType> : Transition
        where StateType : State
    {
        public StateType Origin { get; set; }
        public StateType Destination { get; set; }

        public  StateType GetDestination()
        {
            return this.Destination;
        }
        public  StateType GetOrigin()
        {
            return this.Origin;
        }

    }

}

