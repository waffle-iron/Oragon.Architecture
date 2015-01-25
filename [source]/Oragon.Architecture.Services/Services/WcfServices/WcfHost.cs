using Oragon.Architecture.Extensions;
using Oragon.Architecture.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Oragon.Architecture.Services.WcfServices
{
	public class WcfHost<TServiceType, TServiceInterface>
		where TServiceType : class,  TServiceInterface, new()
	//where ServiceInterface : Object
	{
		#region Private Fields

		private Uri[] _baseAddresses;
		private ServiceHost _host;

		#endregion Private Fields

		#region Public Constructors

		public WcfHost()
		{
		}

		#endregion Public Constructors

		#region Public Properties

		public Uri[] BaseAddresses
		{
			get
			{
				return this._baseAddresses;
			}
			set
			{
				this._baseAddresses = this.AnalyseDynamicPorts(value);
			}
		}

		public ConcurrencyMode ConcurrencyMode { get; set; }

		public InstanceContextMode InstanceContextMode { get; set; }

		public string Name { get; set; }

		public TServiceInterface ServiceInstance { get; set; }

		#endregion Public Properties

		#region Public Methods

		public void Start()
		{
			var serviceInterfaceType = typeof(TServiceInterface);
			WcfServiceHostFactory wcfServiceHostFactory = new WcfServiceHostFactory()
			{
				ConcurrencyMode = this.ConcurrencyMode,
				InstanceContextMode = this.InstanceContextMode,
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
			if (this.ServiceInstance == null)
				this._host = wcfServiceHostFactory.BuildHost(typeof(TServiceType));
			else
				this._host = wcfServiceHostFactory.BuildHost(this.ServiceInstance);
			_host.Open();
		}

		public void Stop()
		{
			if (this._host != null)
			{
				this._host.Close();
				this._host = null;
			}
		}

		#endregion Public Methods

		#region Private Methods

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

		#endregion Private Methods
	}
}