using System;
using System.ServiceModel.Channels;

namespace Oragon.Architecture.Services.WcfServices
{
	public class ServiceEndpointConfiguration
	{
		#region Public Properties

		public Binding Binding { get; set; }

		public string Name { get; set; }

		public Type ServiceInterface { get; set; }

		#endregion Public Properties
	}
}