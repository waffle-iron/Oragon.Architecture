using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.LogEngine.Business.Entity;

namespace Oragon.Architecture.LogEngine.Data.Process
{
	internal partial class LogEntryDataProcess
	{
		internal List<LogEntry> ObterLogsNaoIndexados(int max)
		{
			return this.Query<LogEntry>()
				.Where(it => it.Indexed == false && it.Trash == false)
				.OrderBy(it => it.LogEntryID)
				.Take(max)
				.ToList();
		}

		internal List<LogEntry> ObterLogsDaLixeira(int max)
		{
			return this.Query<LogEntry>()
				.Where(it => it.Trash == true)
				.OrderBy(it => it.LogEntryID)
				.Take(max)
				.ToList();
		}

	}
}
