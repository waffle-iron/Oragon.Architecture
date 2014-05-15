using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Services.WcfServices
{
	public class ServiceEndpointConfiguration
	{
		public Type ServiceInterface { get; set; }

		public Binding Binding { get; set; }

		public string Name { get; set; }

	}
}
