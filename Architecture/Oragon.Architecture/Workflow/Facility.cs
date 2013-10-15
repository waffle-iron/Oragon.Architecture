using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow.Facility
{
    public class StringStateMachine : StateMachine<StringTransition, StringState> { }

    public class StringTransition : Transition<StringState> { }

    public class StringState : State<string> { }

}

namespace Oragon.Architecture.Workflow.Facility
{
    public class IntStateMachine : StateMachine<IntTransition, IntState> { }

    public class IntTransition : Transition<IntState> { }

    public class IntState : State<int> { }
}
