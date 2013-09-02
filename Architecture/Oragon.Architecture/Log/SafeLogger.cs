using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Log
{
	public class SafeLogger: ILogger
	{
		public ILogger PrimaryLogger { get; set; }

		public ILogger SecondaryLogger { get; set; }


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
			try
			{
				this.PrimaryLogger.Log(context, content, logLevel, tags);
			}
			catch(Exception ex)
			{
				this.SecondaryLogger.Log("SafeLogger - ", ex.ToString(), LogLevel.Error, tags);
				this.SecondaryLogger.Log(context, content, logLevel, tags);
			}
		}
	}
}
