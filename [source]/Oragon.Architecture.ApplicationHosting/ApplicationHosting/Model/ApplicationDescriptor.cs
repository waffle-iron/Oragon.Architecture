using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Model
{
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
