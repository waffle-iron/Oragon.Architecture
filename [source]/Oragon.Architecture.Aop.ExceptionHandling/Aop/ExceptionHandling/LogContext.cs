using Oragon.Architecture.Extensions;
using System;
using System.Collections.Generic;

namespace Oragon.Architecture.Aop.ExceptionHandling
{
	public class LogContext : IDisposable
	{
		#region Private Fields

		private bool isDisposed;

		#endregion Private Fields

		#region Public Constructors

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

		#endregion Public Constructors

		#region Public Properties

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

		public bool IsFake { get; private set; }

		public LogContext Parent { get; private set; }

		#endregion Public Properties

		#region Protected Properties

		protected Dictionary<string, string> LogTags { get; private set; }

		#endregion Protected Properties

		#region Public Methods

		public void Dispose()
		{
			if (this.isDisposed == false)
			{
				this.isDisposed = true;
				this.Dispose(true);
			}
		}

		public Dictionary<string, string> GetDictionary()
		{
			Dictionary<string, string> returnValue = new Dictionary<string, string>();
			foreach (var item in this.LogTags)
				returnValue.Add(item.Key, item.Value);
			return returnValue;
		}

		public void Remove(string key)
		{
			this.LogTags.Remove(key);
		}

		public void SetValue(string key, string value)
		{
			this.LogTags.AddOrUpdate(key, value);
		}

		#endregion Public Methods

		#region Private Methods

		private void Dispose(bool dispose)
		{
			if (!this.IsFake)
			{
				LogContext itemToSet = (this.Parent != null) ? this.Parent : null;
				Spring.Threading.LogicalThreadContext.SetData("LogContext", itemToSet);
			}
			if (dispose)
				GC.SuppressFinalize(this);
		}

		#endregion Private Methods
	}
}