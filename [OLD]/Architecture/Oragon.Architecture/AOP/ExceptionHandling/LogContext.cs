using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.AOP.ExceptionHandling
{
	public class LogContext : IDisposable
	{
		protected Dictionary<string, string> LogTags { get; private set; }

		public LogContext Parent { get; private set; }

		public bool IsFake { get; private set; }

		public LogContext(bool isFake = false)
		{
			this.LogTags = new Dictionary<string, string>();
			this.IsFake = isFake;
			if (!this.IsFake)
			{
				this.Parent = Spring.Threading.LogicalThreadContext.GetData("LogContext") as LogContext;
				Spring.Threading.LogicalThreadContext.SetData("LogContext", this);
			}
		}

		public void SetValue(string key, string value)
		{
			this.LogTags.AddOrUpdate(key, value);
		}

		public void Remove(string key)
		{
			this.LogTags.Remove(key);
		}

		public Dictionary<string, string> GetDictionary()
		{
			Dictionary<string, string> returnValue = new Dictionary<string, string>();
			foreach (var item in this.LogTags)
				returnValue.Add(item.Key, item.Value);
			return returnValue;
		}

		public void Dispose()
		{
			if (!this.IsFake)
			{
				LogContext itemToSet = (this.Parent != null) ? this.Parent : null;
				Spring.Threading.LogicalThreadContext.SetData("LogContext", itemToSet);
			}
		}

		public static LogContext Current
		{
			get
			{
				LogContext logContext = Spring.Threading.LogicalThreadContext.GetData("LogContext") as LogContext;
				if (logContext == null)
					logContext = new LogContext(true);
				return logContext;
			}
		}
	}
}
