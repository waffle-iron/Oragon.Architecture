using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.LogEngine.Business.Process;
using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.Log.Model;

namespace Oragon.Architecture.LogEngine.Business.Workflow
{
	internal class LogBusinessWorkflow
	{
		private NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		private LogBusinessProcess LogBP { get; set; }
		private MessageConverterBusinessProcess MessageConverterBP { get; set; }

		internal void SaveLog(string message)
		{
			this.Logger.Trace("LogBusinessWorkflow.SaveLog(message: '{0}') BEGIN", message);
			LogEntryTransferObject logEntryTransferObject = this.MessageConverterBP.ConvertMessage(message);
			LogEntry entry = this.LogBP.SaveLog(logEntryTransferObject);
			this.Logger.Trace("LogBusinessWorkflow.SaveLog(message: '{0}') END", message);
		}

	}
}
