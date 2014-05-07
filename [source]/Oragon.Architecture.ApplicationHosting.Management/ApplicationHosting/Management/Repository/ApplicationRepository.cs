using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository
{
	public class ApplicationRepository
	{
		object syncLock = new Object();

		public List<Machine> Machines { get; private set; }

		public ApplicationRepository()
		{
			this.Machines = new List<Machine>();
		}

		public Host Register(RegisterHostRequestMessage registerMessage)
		{
			lock (syncLock)
			{
				Machine machine = this.Machines.SingleOrDefault(it => it.MachineDescriptor.MachineName == registerMessage.MachineDescriptor.MachineName);
				if (machine == null)
				{
					machine = new Machine()
					{
						MachineDescriptor = registerMessage.MachineDescriptor,
						Hosts = new List<Host>()
					};
					this.Machines.Add(machine);
				}
				else
				{
					machine.MachineDescriptor = registerMessage.MachineDescriptor;
				}
				Host host = new Host() { ID = Guid.NewGuid(), HostDescriptor = registerMessage.HostDescriptor };
				machine.Hosts.Add(host);
				return host;
			}
		}


		public Host Unregister(UnregisterHostRequestMessage unregisterMessage)
		{
			lock (syncLock)
			{
				Host returnValue = null;
				var query = from machine in this.Machines
							from host in machine.Hosts
							where
							host.ID == unregisterMessage.ClientID
							select new { machine = machine, host = host };

				if (query.Any())
				{
					var tuple = query.Single();
					tuple.machine.Hosts.Remove(tuple.host);
					returnValue = tuple.host;
				}
				return returnValue;
			}

		}
	}
}
