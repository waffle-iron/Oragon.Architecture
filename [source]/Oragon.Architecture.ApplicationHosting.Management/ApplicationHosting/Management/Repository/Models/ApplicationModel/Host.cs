using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class Host
	{
		[DataMember]
		public Guid ID { get; set; }

		[DataMember]
		public HostDescriptor HostDescriptor { get; set; }

		private Oragon.Architecture.ApplicationHosting.Services.WcfClient<IHostProcessService> HostProcessServiceClient;

		private System.Timers.Timer heartBeatTimer;

		public Host()
		{
			this.heartBeatTimer = new System.Timers.Timer(1000 * 5);
			this.heartBeatTimer.Elapsed += heartBeatTimer_Elapsed;
		}

		internal void Connect(Uri tcpUri, Uri httpUri)
		{
			this.HostProcessServiceClient = new Oragon.Architecture.ApplicationHosting.Services.WcfClient<IHostProcessService>(serviceName: "HostProcessService", httpEndpointAddress: httpUri, tcpEndpointAddress: tcpUri);
			this.HostProcessServiceClient.Service.HeartBeat();
			this.heartBeatTimer.Start();
		}

		void heartBeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			this.HostProcessServiceClient.Service.HeartBeat();
		}

		internal void Disconnect()
		{
			this.heartBeatTimer.Stop();
			this.HostProcessServiceClient.Dispose();
			this.HostProcessServiceClient = null;
		}
	}
}
