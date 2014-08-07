using System.ServiceModel;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[ServiceContract]
	public interface IHeartBeatService
	{
		#region Public Methods

		[OperationContract]
		void HeartBeat();

		#endregion Public Methods
	}
}