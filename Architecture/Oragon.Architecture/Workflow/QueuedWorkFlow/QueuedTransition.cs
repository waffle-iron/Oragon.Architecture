﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedTransition : Oragon.Architecture.Workflow.Facility.StringTransition
	{
		public string LogicalQueueName { get; set; }
		public string ExchangeName { get; set; }
		public int ConcurrentConsumers { get; set; }
		public object Service { get; set; }
		public string ServiceMethod { get; set; }
		
		public ExceptionStategy Strategy { get; set; }


		public string BuildRoutingKey()
		{
			var origin = this.Origin != null ? this.Origin : string.Empty;
			var destination = this.Destination != null ? this.Destination : string.Empty;

			string returnValue = string.Format("{0}->{1}", origin, destination);
			return returnValue;
		}

		public string BuildFailureRoutingKey()
		{
			var origin = this.Origin != null ? this.Origin : string.Empty;
			var destination = this.Destination != null ? this.Destination : string.Empty;

			string returnValue = string.Format("{0}->{1}#Failure#", origin, destination);
			return returnValue;

		}

	}


	public enum ExceptionStategy
	{
		SendToError,
		SendToNext,
		Requeue
	}

}
