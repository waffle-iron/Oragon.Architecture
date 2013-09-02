using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.LogEngine.Business.Process;
using Oragon.Architecture.LogEngine.Business.Entity;

namespace Oragon.Architecture.LogEngine.Business.Workflow
{
	internal class IndexerBusinessWorkflow
	{
		private NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		private LogBusinessProcess LogBP { get; set; }
		private IndexerBusinessProcess IndexerBP { get; set; }
		private int IndexPageSize { get; set; }
		private int DropIndexPageSize { get; set; }

		internal int IndexNewLogs()
		{
			this.Logger.Trace("IndexerBusinessWorkflow.IndexNewLogs() BEGIN");
			List<LogEntry> logEntryList = this.LogBP.ObterLogsNaoIndexados(this.IndexPageSize);
			if (logEntryList.Count > 0)
			{
				this.Logger.Trace("Indexando {0} logs, entre {1} e {2}.", logEntryList.Count, logEntryList.First().LogEntryID, logEntryList.Last().LogEntryID);
				foreach (LogEntry logEntry in logEntryList)
				{
					this.IndexerBP.AddLogEntryToIndex(logEntry);
					this.LogBP.UpdateLog(logEntry);
				}
				this.Logger.Trace("Foram indexados {0} logs", logEntryList.Count);
			}
			else
				this.Logger.Trace("Nenhum log Indexado");

			this.Logger.Trace("IndexerBusinessWorkflow.IndexNewLogs() END");
			return logEntryList.Count;
		}

		internal int DeleteLogs()
		{
			this.Logger.Trace("IndexerBusinessWorkflow.DeleteLogs() BEGIN");
			List<LogEntry> logEntryList = this.LogBP.ObterLogsDaLixeira(this.DropIndexPageSize);
			if (logEntryList.Count > 0)
			{
				this.Logger.Trace("Excluindo {0} logs, entre {1} e {2}.", logEntryList.Count, logEntryList.First().LogEntryID, logEntryList.Last().LogEntryID);
				this.IndexerBP.RemoveLogEntriesFromIndex(logEntryList);
				foreach (LogEntry logEntry in logEntryList)
				{
					this.LogBP.DeleteLog(logEntry);
				}
				this.Logger.Trace("Foram excluídos {0} logs", logEntryList.Count);
			}
			else
				this.Logger.Trace("Nenhum log excluído");
			this.Logger.Trace("IndexerBusinessWorkflow.DeleteLogs() END");
			return logEntryList.Count;
		}
	}
}
