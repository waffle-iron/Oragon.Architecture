using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Logging
{
	public interface ILogger
	{
		void Debug(string context, string content, params string[] tags);
		void Error(string context, string content, params string[] tags);
		void Fatal(string context, string content, params string[] tags);
		void Trace(string context, string content, params string[] tags);
		void Warn(string context, string content, params string[] tags);
		void Audit(string context, string content, params string[] tags);
		void Log(string context, string content, LogLevel nivelLog, Dictionary<string, string> tags);
	}
}
