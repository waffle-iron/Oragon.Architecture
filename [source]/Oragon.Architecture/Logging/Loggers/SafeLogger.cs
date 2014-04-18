using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Logging.Loggers
{
	public class SafeLogger : AbstractLogger
	{
		public ILogger PrimaryLogger { get; set; }

		public ILogger SecondaryLogger { get; set; }

		protected override void SendLog(LogEntry logEntry)
		{
			List<string> values = new List<string>();
			try
			{
				this.PrimaryLogger.Log(logEntry.Context, logEntry.Content, logEntry.LogLevel, logEntry.Tags);
			}
			catch (Exception ex)
			{
				this.SecondaryLogger.Log("SafeLogger - ", ex.ToString(), LogLevel.Error, logEntry.Tags);
				this.SecondaryLogger.Log(logEntry.Context, logEntry.Content, logEntry.LogLevel, logEntry.Tags);
			}
		}
	}
}
