using System;
using System.ServiceModel.Channels;

namespace Oragon.Architecture.Services.WcfServices
{
	public static class MexBindingResolver
	{
		#region Public Methods

		public static Binding Resolve(MexBindingProtocol protocol)
		{
			System.ServiceModel.Channels.Binding returnValue = null;
			if (protocol == MexBindingProtocol.None)
				returnValue = null;
			else if (protocol == MexBindingProtocol.Http)
				returnValue = System.ServiceModel.Description.MetadataExchangeBindings.CreateMexHttpBinding();
			else if (protocol == MexBindingProtocol.Https)
				System.ServiceModel.Description.MetadataExchangeBindings.CreateMexHttpsBinding();
			else if (protocol == MexBindingProtocol.NamedPipe)
				System.ServiceModel.Description.MetadataExchangeBindings.CreateMexNamedPipeBinding();
			else if (protocol == MexBindingProtocol.Tcp)
				System.ServiceModel.Description.MetadataExchangeBindings.CreateMexTcpBinding();
			else
				throw new InvalidOperationException("Tipo de Protocolo não suportado ou não configurado");
			return returnValue;
		}

		#endregion Public Methods
	}
}