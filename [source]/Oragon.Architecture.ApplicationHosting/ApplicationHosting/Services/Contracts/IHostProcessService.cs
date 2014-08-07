using System.ServiceModel;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[ServiceContract]
	public interface IHostProcessService : IHeartBeatService
	{
		#region Public Methods

		[OperationContract]
		void AddApplication();

		[OperationContract]
		HostStatistic CollectStatistics();

		[OperationContract]
		void StartApplication();

		[OperationContract]
		void StopApplication();

		#endregion Public Methods
	}
}