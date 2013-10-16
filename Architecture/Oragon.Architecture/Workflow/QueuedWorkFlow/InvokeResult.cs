using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class InvokeResult
	{
		public object ReturnedValue { get; set; }
		public Exception Exception { get; set; }


		public bool HasValue
		{
			get
			{
				return (this.ReturnedValue != null && ((this.ReturnedValue is System.Reflection.Missing) == false));
			}
		}

		public bool Success
		{
			get
			{
				return (this.Exception == null);
			}
		}
	}
}
