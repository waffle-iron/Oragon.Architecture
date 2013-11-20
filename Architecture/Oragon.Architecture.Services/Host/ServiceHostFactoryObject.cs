
using Spring.Objects.Factory;
using System;
using Spring.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel.Description;
namespace Oragon.Architecture.Services.Host
{
	public class ServiceHostFactoryObject : IFactoryObject, IInitializingObject, IObjectFactoryAware, System.IDisposable, IService
	{
		public List<IServiceBehavior> Behaviors { get; set; }
		public List<ServiceEndpointConfiguration> ServiceEndpoints { get; set; }

		private bool _useServiceProxyTypeCache = true;
		private Uri[] _baseAddresses = new Uri[0];
		protected IObjectFactory objectFactory;
		protected SpringServiceHost springServiceHost;
		public string TargetName { get; set; }
		public Uri[] BaseAddresses { get; set; }
		public bool UseServiceProxyTypeCache { get; set; }
		public virtual IObjectFactory ObjectFactory { get; set; }
		public virtual System.Type ObjectType { get { return typeof(SpringServiceHost); } }
		public virtual bool IsSingleton { get { return false; } }

		public virtual object GetObject()
		{
			return this.springServiceHost;
		}
		public virtual void AfterPropertiesSet()
		{
			this.ValidateConfiguration();
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

		public void Dispose()
		{
			this.Stop();
		}
		protected virtual void ValidateConfiguration()
		{
			if (this.TargetName == null)
			{
				throw new System.ArgumentException("The TargetName property is required.");
			}
		}

		public string Name { get; set; }

		public void Start()
		{
			this.springServiceHost = new SpringServiceHost(this.TargetName, this.objectFactory, this.UseServiceProxyTypeCache, this.BaseAddresses);
			this.ConfigureHost();
			this.springServiceHost.Open();
		}

		public void Stop()
		{
			if (this.springServiceHost != null)
			{
				this.springServiceHost.Close();
				this.springServiceHost = null;
			}
		}
	}
}
