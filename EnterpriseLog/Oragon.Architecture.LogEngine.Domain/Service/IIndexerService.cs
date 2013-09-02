using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Oragon.Architecture.LogEngine.Service
{
	[ServiceContract]
	public interface IIndexerService
	{
		[OperationContract]
		int IndexNewLogs();

		[OperationContract]
		int DeleteLogs();
	}
}
