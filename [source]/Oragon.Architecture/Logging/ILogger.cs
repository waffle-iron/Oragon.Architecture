using System.Collections.Generic;

namespace Oragon.Architecture.Logging
{
	public interface ILogger
	{
		#region Public Methods

		void Audit(string context, string content, params string[] tags);

		void Debug(string context, string content, params string[] tags);

		void Error(string context, string content, params string[] tags);

		void Fatal(string context, string content, params string[] tags);

		void Log(string context, string content, LogLevel logLevel, Dictionary<string, string> tags);

		void Trace(string context, string content, params string[] tags);

		void Warn(string context, string content, params string[] tags);

		#endregion Public Methods
	}
}