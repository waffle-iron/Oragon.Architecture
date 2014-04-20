using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public class ApplicationHostingConfiguration
	{
		[Required]
		public string ConfigurationFile { get; set; }
		[Required]
		public string BaseDirectory { get; set; }

	}
}
