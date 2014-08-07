using Oragon.Architecture.Extensions;
using System;
using System.ServiceModel;

namespace Oragon.Architecture.Services.WcfServices
{
	public class WcfClient<ServiceInterface> : IDisposable
	{
		#region Private Fields

		private ChannelFactory<ServiceInterface> channelFactory;

		#endregion Private Fields

		#region Public Constructors

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

		#endregion Public Constructors

		#region Public Properties

		public ServiceInterface Service { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void Dispose()
		{
			((ICommunicationObject)this.Service).Close();
			this.channelFactory.Close();
		}

		#endregion Public Methods
	}
}