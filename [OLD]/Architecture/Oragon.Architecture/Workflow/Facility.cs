using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow.Facility
{
    public class StringStateMachine : StateMachine<StringTransition, string> { }

    public class StringTransition : Transition<string> { }


}

namespace Oragon.Architecture.Workflow.Facility
{
    public class IntStateMachine : StateMachine<IntTransition, int> { }

    public class IntTransition : Transition<int> { }

}
