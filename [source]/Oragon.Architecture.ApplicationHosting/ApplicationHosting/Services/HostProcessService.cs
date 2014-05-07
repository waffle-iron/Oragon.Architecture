using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Services
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
	public class HostProcessService : IHostProcessService
	{
		public void CollectStatistics()
		{
			
		}

		public void AddApplication()
		{
			
		}

		public void StartApplication()
		{
			
		}

		public void StopApplication()
		{
			
		}

		public void HeartBeat()
		{
			
		}
	}
}
