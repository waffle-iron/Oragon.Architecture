using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oragon.Architecture.Log.Model;
using Spring.Messaging.Amqp.Core;

namespace Oragon.Architecture.Log
{
	public class AMQPLogger : AbstractLogger
	{
		protected IAmqpTemplate Template { get; set; }

		protected override void SendLog(LogEntryTransferObject logEntry)
		{
			object messageObject = JsonConvert.SerializeObject(logEntry, Formatting.Indented);
			Func<Message, Message> messagePostProcessor = it => {
				it.MessageProperties.SetHeader("LogLevelID", (int)logEntry.LogLevel);
				it.MessageProperties.SetHeader("LogLevelName", logEntry.LogLevel.ToString());
				return it;
			};
			this.Template.ConvertAndSend(messageObject, messagePostProcessor);
		}
	}
}
