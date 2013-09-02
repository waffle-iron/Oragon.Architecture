using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace Oragon.Architecture.Services.Host
{
	public class ServiceEndpointConfiguration
	{
		public Type ServiceInterface { get; set; }

		public Binding Binding { get; set; }

		public string Name { get; set; }

	}
}
