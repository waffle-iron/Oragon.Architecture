using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.ServiceModel;
using System.ServiceModel.Description;

namespace Oragon.Architecture.Services.Host
{
	public class ConfigurableServiceHostFactoryObject : Spring.ServiceModel.Activation.ServiceHostFactoryObject
	{

		public List<IServiceBehavior> Behaviors { get; set; }

		public List<ServiceEndpointConfiguration> ServiceEndpoints { get; set; }

		public override void AfterPropertiesSet()
		{
			ValidateConfiguration();

			this.springServiceHost = new SpringServiceHost(TargetName, objectFactory, UseServiceProxyTypeCache, BaseAddresses);

			this.ConfigureHost();
		}


		protected virtual void ConfigureHost()
		{
			this.AddBehaviors();
			this.AddServiceEndpoints();
		}


		protected void AddBehaviors()
		{
			if (this.Behaviors != null && this.Behaviors.Count > 0)
			{
				foreach (IServiceBehavior currentServiceBehavior in this.Behaviors)
				{
					this.springServiceHost.Description.Behaviors.Add(currentServiceBehavior);
				}
			}
		}

		protected void AddServiceEndpoints()
		{
			if (this.ServiceEndpoints != null && this.ServiceEndpoints.Count > 0)
			{
				foreach (ServiceEndpointConfiguration currentServiceEndpoint in this.ServiceEndpoints)
				{
					this.springServiceHost.AddServiceEndpoint(currentServiceEndpoint.ServiceInterface, currentServiceEndpoint.Binding, currentServiceEndpoint.Name);
				}
			}
		}



		
	}
}
