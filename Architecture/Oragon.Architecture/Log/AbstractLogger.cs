using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Messaging.Core;
using Newtonsoft.Json;
using Oragon.Architecture.Log.Model;

namespace Oragon.Architecture.Log
{
	public abstract class AbstractLogger : Oragon.Architecture.Log.ILogger
	{
        protected Dictionary<string, string> AdditionalMetadata { get; set; }

		public void Debug(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Debug, tags);
		}

		public void Trace(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Trace, tags);
		}

		public void Warn(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Warn, tags);
		}

		public void Error(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Error, tags);
		}

		public void Fatal(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Fatal, tags);
		}

        public void Audit(string context, string content, params string[] tags)
        {
			this.Log(context, content, LogLevel.Audit, tags);
        }

		public void Log(string context, string content, LogLevel logLevel, params string[] tags)
		{
			Dictionary<string, string> tagsDic = new Dictionary<string, string>();
			if (tags.Length > 0)
			{
				if (tags.Length % 2 != 0)
					throw new InvalidOperationException("Tags não possui uma quantidade de valores par;");
				else
				{
					int keyIndex = 0;
					int valueIndex = 1;
					for (; valueIndex < tags.Length; keyIndex += 2, valueIndex += 2)
					{
						tagsDic.Add("User:" + tags[keyIndex], tags[valueIndex]);
					}
				}
			}
			this.Log(context, content, logLevel, tagsDic);
		}

		private void Log(string context, string content, LogLevel logLevel, Dictionary<string, string> tags)
		{
            if (this.AdditionalMetadata != null)
            {
                foreach (var additionalMetadataItem in this.AdditionalMetadata)
                {
                    tags.Add("Meta:" + additionalMetadataItem.Key, additionalMetadataItem.Value);
                }
            }
			
          
			LogEntryTransferObject logEntry = new LogEntryTransferObject()
			{
				LogEntryID = 0,
				Content = content,
				Date = DateTime.Now,
				LogLevel = logLevel,
				Tags = tags
			};
			this.SendLog(logEntry);
		}

		protected abstract void SendLog(LogEntryTransferObject logEntry);
	}
}
