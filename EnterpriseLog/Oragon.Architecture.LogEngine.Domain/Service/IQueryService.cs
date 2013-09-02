using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Oragon.Architecture.LogEngine.Business.Entity;

namespace Oragon.Architecture.LogEngine.Service
{
	[ServiceContract]
	public interface IQueryService
	{
		[OperationContract]
		List<LogEntry> Search(string searchExpression, int hitsPerPage);

		[OperationContract]
		List<LogEntry> SearchByExample(LogEntry example, int hitsPerPage);
	}
}
