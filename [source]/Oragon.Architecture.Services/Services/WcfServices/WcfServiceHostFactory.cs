
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Services.WcfServices
{
	public class WcfServiceHostFactory
	{
		public ConcurrencyMode ConcurrencyMode { get; set; }
		public InstanceContextMode InstanceContextMode { get; set; }

		public Uri[] BaseAddresses { get; set; }

		public List<IServiceBehavior> Behaviors { get; set; }

		public List<ServiceEndpointConfiguration> ServiceEndpoints { get; set; }
		public virtual ServiceHost BuildHost(object instance)
		{
			ServiceHost serviceHost = new ServiceHost(instance, this.BaseAddresses);
			this.ConfigureHost(serviceHost);
			return serviceHost;
		}

		public virtual ServiceHost BuildHost(Type serviceType)
		{
			ServiceHost serviceHost = new ServiceHost(serviceType, this.BaseAddresses);
			this.ConfigureHost(serviceHost);
			return serviceHost;
		}

		protected virtual void ConfigureHost(ServiceHost serviceHost)
		{
			this.AddBehaviors(serviceHost);
			this.AddServiceEndpoints(serviceHost);
			this.ConfigureServiceBehaviorAttribute(serviceHost);
		}

		protected virtual void AddBehaviors(ServiceHost serviceHost)
		{
			if (this.Behaviors != null && this.Behaviors.Count > 0)
			{
				foreach (IServiceBehavior currentServiceBehavior in this.Behaviors)
				{
					serviceHost.Description.Behaviors.Add(currentServiceBehavior);
				}
			}
		}

		protected virtual void AddServiceEndpoints(ServiceHost serviceHost)
		{
			if (this.ServiceEndpoints != null && this.ServiceEndpoints.Count > 0)
			{
				foreach (ServiceEndpointConfiguration currentServiceEndpoint in this.ServiceEndpoints)
				{
					serviceHost.AddServiceEndpoint(currentServiceEndpoint.ServiceInterface, currentServiceEndpoint.Binding, currentServiceEndpoint.Name);
				}
			}
		}

		protected virtual void ConfigureServiceBehaviorAttribute(ServiceHost serviceHost)
		{
			var attr = serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
			attr.ConcurrencyMode = this.ConcurrencyMode;
			attr.InstanceContextMode = this.InstanceContextMode;
		}
		
	}
}
