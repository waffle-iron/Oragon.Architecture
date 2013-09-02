using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Log.Model;

namespace Oragon.Architecture.Log
{
	public class DiagnosticsLogger : AbstractLogger
	{
		protected override void SendLog(LogEntryTransferObject logEntry)
		{
			System.Diagnostics.Debug.WriteLine("Log-----{0}", new object[] { logEntry.LogLevel.ToString() });
			System.Diagnostics.Debug.WriteLine("\t{0}", new object[] { logEntry.Content });
			System.Diagnostics.Debug.WriteLine("\t{0}", new object[] { logEntry.Date.ToLongDateString() });
			System.Diagnostics.Debug.WriteLine("\t{0}", new object[] { logEntry.Date.ToLongTimeString() });
			foreach (KeyValuePair<string, string> item in logEntry.Tags)
			{
				System.Diagnostics.Debug.WriteLine("\t\tKey  :{0}", new object[] { item.Key });
				System.Diagnostics.Debug.WriteLine("\t\tValue:{0}", new object[] { item.Value });
			}
			System.Diagnostics.Debug.WriteLine("---------------------------------");
		}
	}
}
