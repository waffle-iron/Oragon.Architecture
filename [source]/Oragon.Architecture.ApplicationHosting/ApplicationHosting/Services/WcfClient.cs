using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Services
{
	public class WcfHost<ServiceType>
	{
		private ServiceHost host;

		public void Start(params Uri[] baseAddresses)
		{
			this.host = new ServiceHost(typeof(ServiceType), baseAddresses);

			host.Open(new TimeSpan(0, 1, 0));
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
