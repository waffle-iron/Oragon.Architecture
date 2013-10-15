using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
    public class QueuedTransition : Oragon.Architecture.Workflow.Facility.StringTransition
    {
        public string QueueToListen { get; set; }
        public string QueueToReportError { get; set; }
        public string ExchangeName { get; set; }
        public int ConcurrentConsumers { get; set; }
        public object Service { get; set; }
        public string ServiceMethod { get; set; }

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

            string returnValue = string.Format("{0}->{1}#FailOn#", origin, destination);
            return returnValue;
        }


        internal Spring.Messaging.Amqp.Core.IExchange Exchange { get; set; }

        internal Spring.Messaging.Amqp.Core.Queue ProcessQueue { get; set; }
        internal Spring.Messaging.Amqp.Core.Queue FailureQueue { get; set; }

        internal Spring.Messaging.Amqp.Core.Binding ProcessBinding { get; set; }
        internal Spring.Messaging.Amqp.Core.Binding FailureBinding { get; set; }

    }

}
