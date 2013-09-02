using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.MsmqIntegration;
using Oragon.Architecture.LogEngine.Business.Entity;

namespace Oragon.Architecture.LogEngine.Service
{

	[ServiceContract]
	public interface ILogService 
	{
		[OperationContract]
		void Handler(string logEntryTransferObject);
	}
}
