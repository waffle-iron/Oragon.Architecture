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
		where ServiceType : class,  ServiceInterface, new()
		//where ServiceInterface : Object
	{
		private ServiceHost host;

		public string Name { get; set; }

		public Uri[] BaseAddresses { get; set; }

		public ServiceInterface ServiceInstance { get; set; }

		public WcfHost(string name, params Uri[] baseAddresses)
		{
			this.Name = name;
			this.BaseAddresses = this.AnalyseDynamicPorts(baseAddresses);
		}

		public void Start()
		{
			var serviceInterfaceType = typeof(ServiceInterface);
			WcfServiceHostFactory wcfServiceHostFactory = new WcfServiceHostFactory()
			{
				BaseAddresses = this.BaseAddresses,
				Behaviors = new List<System.ServiceModel.Description.IServiceBehavior>() { 
					new System.ServiceModel.Description.ServiceMetadataBehavior(){ HttpGetEnabled = true},
				},
				ServiceEndpoints = new List<ServiceEndpointConfiguration>()
				{
					WcfHelper.BuildEndpoint(WcfHelper.EndpointType.Http, this.Name, serviceInterfaceType),
					WcfHelper.BuildEndpoint(WcfHelper.EndpointType.NetTcp, this.Name, serviceInterfaceType),
					WcfHelper.BuildEndpoint(WcfHelper.EndpointType.Mex, this.Name, serviceInterfaceType),
				}
			};

			if (this.ServiceInstance != null)
				this.host = wcfServiceHostFactory.BuildHost(typeof(ServiceType));
			else
				this.host = wcfServiceHostFactory.BuildHost(this.ServiceInstance);
			host.Open();
		}

		private Uri[] AnalyseDynamicPorts(Uri[] baseAddresses)
		{
			Uri[] reurnValue = baseAddresses.ToArray();
			if (reurnValue.Any(it => it.Port == 0))
			{
				for (int i = 0; i < reurnValue.Length; i++)
				{
					if (reurnValue[i].Port == 0)
					{
						int dynamicPort = DynamicPort.GetFreePort();
						reurnValue[i] = reurnValue[i].Port(dynamicPort);
					}
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
