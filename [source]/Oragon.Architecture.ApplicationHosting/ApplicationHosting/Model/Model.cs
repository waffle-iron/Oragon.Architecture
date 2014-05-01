using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Model
{
	public class HostDescriptor
	{
		public Guid ID { get; set; }
		public string MachineName { get; set; }

		public string Name { get; set; }

		public string FriendlyName { get; set; }

		public string Description { get; set; }

		public List<ApplicationDescriptor> Applications { get; set; }

		public List<string> IPAddressList { get; set; }

		public int PID { get; set; }
	}


	public class ApplicationDescriptor
	{
		public string Name { get; set; }
		public string FriendlyName { get; set; }
		public string Description { get; set; }
		public string FactoryType { get; set; }
		public string ApplicationConfigurationFile { get; set; }
		public string ApplicationBaseDirectory { get; set; }
	}

}
