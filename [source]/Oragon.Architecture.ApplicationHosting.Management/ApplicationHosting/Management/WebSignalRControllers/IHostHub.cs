using System;
using System.ServiceModel;
namespace Oragon.Architecture.ApplicationHosting.Management.WebSignalRControllers
{
	[ServiceContract]
	public interface IHostHub
	{
		[OperationContract]
		Oragon.Architecture.ApplicationHosting.Services.Contracts.RegisterHostResponseMessage RegisterHost(Oragon.Architecture.ApplicationHosting.Services.Contracts.RegisterHostRequestMessage request);
		[OperationContract]
		Oragon.Architecture.ApplicationHosting.Services.Contracts.UnregisterHostResponseMessage UnregisterHost(Oragon.Architecture.ApplicationHosting.Services.Contracts.UnregisterHostRequestMessage request);

		[OperationContract]
		void Teste1(string argument);

		[OperationContract]
		int Teste2(string argument);
	}

	[ServiceContract]
	public interface IHostHubClient
	{
		[OperationContract]
		void Teste(string texto, int nome);
	}


}
