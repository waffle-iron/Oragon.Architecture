using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow.Facility
{
	public class StringStateMachine : StateMachine<string> { }

	public class StringTransition : Transition<string> { }

	public class StringState : State<string> { }

}

namespace Oragon.Architecture.Workflow.Facility
{
	public class IntStateMachine : StateMachine<int> { }

	public class IntTransition : Transition<int> { }

	public class IntState : State<int> { }
}
