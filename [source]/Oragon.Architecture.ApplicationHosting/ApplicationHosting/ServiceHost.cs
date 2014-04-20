using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public class ServiceHost
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string FriendlyName { get; set; }
		[Required]
		public string Description { get; set; }

		[Required]
		public WindowsServicesConfiguration WindowsServicesConfiguration { get; set; }

		[Required]
		public ApplicationHostingConfiguration ApplicationHostingConfiguration { get; set; }

	}
}
