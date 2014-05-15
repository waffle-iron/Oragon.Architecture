using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;


namespace Oragon.Architecture.Services.WcfServices
{
	public class WcfClient<ServiceInterface> : IDisposable
	{
		public ServiceInterface Service { get; private set; }

		private ChannelFactory<ServiceInterface> channelFactory;

		public WcfClient(string serviceName, Uri tcpEndpointAddress, Uri httpEndpointAddress)
		{
			tcpEndpointAddress = tcpEndpointAddress.Path("/{0}/".FormatWith(serviceName));
			httpEndpointAddress = httpEndpointAddress.Path("/{0}/".FormatWith(serviceName));
			try
			{
				this.channelFactory = new ChannelFactory<ServiceInterface>(WcfHelper.BuildNetTcpBinding(), new EndpointAddress(tcpEndpointAddress.ToString()));
				this.Service = this.channelFactory.CreateChannel();
			}
			catch
			{

				if (this.Service != null)
					((ICommunicationObject)this.Service).Abort();

				if (this.channelFactory != null)
					this.channelFactory.Close();

				try
				{
					this.channelFactory = new ChannelFactory<ServiceInterface>(WcfHelper.BuildBasicHttpBinding(), new EndpointAddress(httpEndpointAddress.ToString()));
					this.Service = this.channelFactory.CreateChannel();
				}
				catch
				{

					if (this.Service != null)
						((ICommunicationObject)this.Service).Abort();

					if (this.channelFactory != null)
						this.channelFactory.Close();

					throw;
				}
			}
		}

		public void Dispose()
		{
			((ICommunicationObject)this.Service).Close();
			this.channelFactory.Close();
		}
	}
}
