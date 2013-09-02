using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.LogEngine.Business.Process;

namespace Oragon.Architecture.LogEngine.Service
{
	public class QueryService : IQueryService
	{
		#region Injeção de Dependência

		private QueryIndexBusinessProcess QueryIndexBP { get; set; }

		#endregion

		public List<LogEntry> Search(string searchExpression, int hitsPerPage)
		{
			List<LogEntry> returnValue = this.QueryIndexBP.Query(searchExpression, hitsPerPage);
			return returnValue;
		}

		public List<LogEntry> SearchByExample(LogEntry example, int hitsPerPage)
		{
			List<LogEntry> returnValue = this.QueryIndexBP.QueryByExample(example, hitsPerPage);
			return returnValue;
		}

	}
}
