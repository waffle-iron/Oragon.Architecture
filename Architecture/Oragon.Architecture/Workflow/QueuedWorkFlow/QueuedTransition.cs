using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedTransition: Oragon.Architecture.Workflow.Facility.StringTransition
	{
		public string QueueToListen { get; set; }
		public string QueueToReportError { get; set; }
		public int ListenerCount { get; set; }
		public object Service { get; set; }
		public string ServiceMethod { get; set; }
	}
	
}
