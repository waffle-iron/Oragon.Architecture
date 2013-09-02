using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Oragon.Architecture;
using Spring.Messaging.Support.Converters;

namespace Oragon.Architecture.Messaging.Support.Converters
{
	public class JsonMessageConverter : IMessageConverter
	{
		public Type[] TargetTypes { get; set; }

		private JsonMessageFormatter messageFormatter;

		public JsonMessageConverter()
		{
			this.messageFormatter = new JsonMessageFormatter();
		}

		public object FromMessage(System.Messaging.Message message)
		{
			this.messageFormatter.TargetTypes = this.TargetTypes;
			message.Formatter = this.messageFormatter;
			return message.Body;
		}

		public System.Messaging.Message ToMessage(object obj)
		{
			this.messageFormatter.TargetTypes = this.TargetTypes;
			return new System.Messaging.Message
			{
				Body = obj,
				Formatter = this.messageFormatter
			};
		}

		public object Clone()
		{
			return new JsonMessageConverter()
			{
				TargetTypes = this.TargetTypes
			};
		}

		
	}
}
