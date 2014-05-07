using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository
{
	public class ApplicationRepository
	{
		public Dictionary<Guid, HostDescriptor> Clients { get; private set; }

		public ApplicationRepository()
		{
			this.Clients = new Dictionary<Guid, HostDescriptor>();
		}

		public Guid Register(HostDescriptor hostDescriptor)
		{
			Guid ID = Guid.NewGuid();
			this.Clients.Add(ID, hostDescriptor);
			return ID;
		}

		public void Unregister(Guid ID)
		{
			this.Clients.Remove(ID);
		}
	}
}
