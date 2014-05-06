using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Services
{
	public class WcfClient<ServiceInterface> : IDisposable
	{
		public ServiceInterface Service { get; private set; }

		private ChannelFactory<ServiceInterface> channelFactory;


		public WcfClient(string endpointAddress)
		{
			var myEndpoint = new EndpointAddress(endpointAddress);
			this.channelFactory = new ChannelFactory<ServiceInterface>(WcfHelper.BuildNetTcpBinding(), myEndpoint);
			try
			{
				this.Service = this.channelFactory.CreateChannel();
			}
			catch
			{
				if (this.Service != null)
				{
					((ICommunicationObject)this.Service).Abort();
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
