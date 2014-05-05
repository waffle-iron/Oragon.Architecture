using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[ServiceContract]
	public interface IHostProcessService
	{
		[OperationContract]
		void CollectStatistics();
		
		[OperationContract]
		void AddApplication();
		
		[OperationContract]
		void StartApplication();

		[OperationContract]
		void StopApplication();

		[OperationContract]
		void HeartBeat();

	}
}
