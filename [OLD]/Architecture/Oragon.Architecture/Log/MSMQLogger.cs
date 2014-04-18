using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Messaging.Core;
using Oragon.Architecture.Log.Model;
using Newtonsoft.Json;

namespace Oragon.Architecture.Log
{
	public class MSMQLogger : AbstractLogger
	{
		protected MessageQueueTemplate Template { get; set; }

		protected override void SendLog(LogEntryTransferObject logEntry)
		{
			this.Template.ConvertAndSend(JsonConvert.SerializeObject(logEntry, Formatting.Indented));
		}

	}
}
