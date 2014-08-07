﻿using Oragon.Architecture.Extensions;
using Oragon.Architecture.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Oragon.Architecture.Services.WcfServices
{
	public class WcfHost<ServiceType, ServiceInterface>
		where ServiceType : class,  ServiceInterface, new()
	//where ServiceInterface : Object
	{
		#region Private Fields

		private Uri[] _baseAddresses;
		private ServiceHost host;

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

		public ServiceInterface ServiceInstance { get; set; }

		#endregion Public Properties

		#region Public Methods

		public void Start()
		{
			var serviceInterfaceType = typeof(ServiceInterface);
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
				this.host = wcfServiceHostFactory.BuildHost(typeof(ServiceType));
			else
				this.host = wcfServiceHostFactory.BuildHost(this.ServiceInstance);
			host.Open();
		}

		public void Stop()
		{
			if (this.host != null)
			{
				this.host.Close();
				this.host = null;
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