using Oragon.Architecture.ApplicationHosting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository
{
	public class ApplicationRepository
	{
		public List<HostDescriptor> Clients { get; private set; }

		public ApplicationRepository()
		{
			this.Clients = new List<HostDescriptor>();
		}

		public Guid Register(HostDescriptor hostDescriptor)
		{
			hostDescriptor.ID = Guid.NewGuid();
			this.Clients.Add(hostDescriptor);
			return hostDescriptor.ID;
		}

		public void Unregister(Guid ID)
		{
			this.Clients.Remove(it => it.ID == ID);
		}
	}
}
