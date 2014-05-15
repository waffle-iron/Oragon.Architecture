using Oragon.Architecture.Services.WcfServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Services.WcfServices
{
	internal static class WcfHelper
	{
		internal enum EndpointType
		{
			NetTcp,
			Http,
			Mex
		}

		internal static System.ServiceModel.BasicHttpBinding BuildBasicHttpBinding()
		{
			return new System.ServiceModel.BasicHttpBinding()
			{
				MaxReceivedMessageSize = 2147483647,
				MaxBufferSize = 2147483647,
				MaxBufferPoolSize = 2147483647
			};
		}

		internal static System.ServiceModel.NetTcpBinding BuildNetTcpBinding()
		{
			return new System.ServiceModel.NetTcpBinding()
			{
				OpenTimeout = new TimeSpan(0, 1, 0),
				CloseTimeout = new TimeSpan(0, 1, 0),
				SendTimeout = new TimeSpan(0, 30, 0),
				ReceiveTimeout = new TimeSpan(0, 30, 0),
				TransferMode = System.ServiceModel.TransferMode.Streamed,
				PortSharingEnabled = true,
				ListenBacklog = 32,
				MaxConnections = 100,
				MaxBufferSize = 2147483647,
				MaxReceivedMessageSize = 2147483647,
				MaxBufferPoolSize = 2147483647,
				Security = new NetTcpSecurity()
				{
					Mode = SecurityMode.None
				},
				ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max
			};
		}

		internal static ServiceEndpointConfiguration BuildEndpoint(EndpointType type, string name, Type serviceInterfaceType)
		{
			ServiceEndpointConfiguration returnValue = null;
			switch (type)
			{
				case EndpointType.Http:
					returnValue = new ServiceEndpointConfiguration()
					{
						ServiceInterface = serviceInterfaceType,
						Name = name,
						Binding = BuildBasicHttpBinding()
					};
					break;
				case EndpointType.NetTcp:
					returnValue = new ServiceEndpointConfiguration()
					{
						ServiceInterface = serviceInterfaceType,
						Name = name,
						Binding = BuildNetTcpBinding()
					};
					break;
				case EndpointType.Mex:
					returnValue = new ServiceEndpointConfiguration()
					{
						ServiceInterface = typeof(System.ServiceModel.Description.IMetadataExchange),
						Name = "mex",
						Binding = MexBindingResolver.Resolve(MexBindingProtocol.Http)
					};
					break;
			}
			return returnValue;
		}
	}
}
