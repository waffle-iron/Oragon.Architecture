using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository
{
	public class ApplicationRepository
	{
		#region Private Fields

		private object syncLock = new Object();

		#endregion Private Fields

		#region Public Constructors

		public ApplicationRepository()
		{
		}

		#endregion Public Constructors

		#region Public Properties

		public string ApplicationRepositoryPath { get; set; }

		public List<Machine> Machines { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public static Guid CreateNewTempApplication()
		{
			throw new NotImplementedException();
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
							host.ID == unregisterMessage.ClientId
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

		#endregion Public Methods
	}
}