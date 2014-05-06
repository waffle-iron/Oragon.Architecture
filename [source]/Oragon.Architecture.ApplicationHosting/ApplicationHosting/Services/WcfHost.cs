using Oragon.Architecture.Networking;
using Oragon.Architecture.Services.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Services
{
	public class WcfHost<ServiceType, ServiceInterface>
		where ServiceType : ServiceInterface
	{
		private ServiceHost host;

		public string Name { get; set; }

		public WcfHost(string name)
		{
			this.Name = name;
		}

		public void Start(params Uri[] baseAddresses)
		{
			Uri[] urlsToBase = this.AnalyseDynamicPorts(baseAddresses);

			WcfServiceHostFactory wcfServiceHostFactory = new WcfServiceHostFactory()
			{
				BaseAddresses = urlsToBase,
				Behaviors = new List<System.ServiceModel.Description.IServiceBehavior>() { 
					new System.ServiceModel.Description.ServiceMetadataBehavior(){ HttpGetEnabled = true}
				},
				ServiceEndpoints = new List<ServiceEndpointConfiguration>()
				{
					WcfHelper.BuildEndpoint<ServiceInterface>(WcfHelper.EndpointType.Http, this.Name),
					WcfHelper.BuildEndpoint<ServiceInterface>(WcfHelper.EndpointType.NetTcp, this.Name),
					WcfHelper.BuildEndpoint<ServiceInterface>(WcfHelper.EndpointType.Mex, this.Name),
				}
			};
			this.host = wcfServiceHostFactory.BuildHost(typeof(ServiceType));
			host.Open();
		}

		private Uri[] AnalyseDynamicPorts(Uri[] baseAddresses)
		{
			Uri[] reurnValue = null;
			Array.Copy(baseAddresses, reurnValue, baseAddresses.Length);

			if (reurnValue.Any(it => it.Port == 0))
			{
				int dynamicPort = DynamicPort.GetFreePort();
				for (int i = 0; i < reurnValue.Length; i++)
				{
					if (reurnValue[i].Port == 0)
						reurnValue[i] = reurnValue[i].Port(dynamicPort);
				}
			}
			return reurnValue;
		}

		public void Stop()
		{
			if (this.host != null)
			{
				this.host.Close();
				this.host = null;
			}
		}
	}
}
