﻿using Oragon.Architecture.Extensions;
using System;
using System.Collections.Generic;

namespace Oragon.Architecture.Logging
{
	public abstract class AbstractLogger : ILogger
	{
		#region Protected Properties

		protected Dictionary<string, string> AdditionalMetadata { get; set; }

		#endregion Protected Properties

		#region Public Methods

		public void Audit(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Audit, tags);
		}

		public void Debug(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Debug, tags);
		}

		public void Error(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Error, tags);
		}

		public void Fatal(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Fatal, tags);
		}

		public void Log(string context, string content, LogLevel logLevel, params string[] tags)
		{
			this.Log(context, content, logLevel, tags.ToDictionary());
		}

		public void Log(string context, string content, LogLevel logLevel, Dictionary<string, string> tags)
		{
			if (this.AdditionalMetadata != null)
			{
				foreach (var additionalMetadataItem in this.AdditionalMetadata)
				{
					tags.Add(additionalMetadataItem.Key, additionalMetadataItem.Value);
				}
			}
			LogEntry logEntry = new LogEntry()
			{
				LogEntryId = 0,
				Context = context,
				Content = content,
				Date = DateTime.Now,
				LogLevel = logLevel,
				Tags = tags
			};
			this.SendLog(logEntry);
		}

		public void Trace(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Trace, tags);
		}

		public void Warn(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Warn, tags);
		}

		#endregion Public Methods

		#region Protected Methods

		protected abstract void SendLog(LogEntry logEntry);

		#endregion Protected Methods
	}
}