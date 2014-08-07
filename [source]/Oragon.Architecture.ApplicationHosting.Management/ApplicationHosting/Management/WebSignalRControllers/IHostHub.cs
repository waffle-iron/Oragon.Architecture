using System.ServiceModel;

namespace Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers
{
	[ServiceContract]
	public interface IHostHub
	{
		#region Public Methods

		[OperationContract]
		Oragon.Architecture.ApplicationHosting.Services.Contracts.RegisterHostResponseMessage RegisterHost(Oragon.Architecture.ApplicationHosting.Services.Contracts.RegisterHostRequestMessage request);

		[OperationContract]
		void Teste1(string argument);

		[OperationContract]
		int Teste2(string argument);

		[OperationContract]
		Oragon.Architecture.ApplicationHosting.Services.Contracts.UnregisterHostResponseMessage UnregisterHost(Oragon.Architecture.ApplicationHosting.Services.Contracts.UnregisterHostRequestMessage request);

		#endregion Public Methods
	}

	[ServiceContract]
	public interface IHostHubClient
	{
		#region Public Methods

		[OperationContract]
		void Teste(string texto, int nome);

		#endregion Public Methods
	}
}