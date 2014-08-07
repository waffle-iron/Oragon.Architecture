using Spring.Objects.Factory;
using System;

namespace Oragon.Architecture.Services.WcfServices
{
	public class MexBindingFactory : IFactoryObject
	{
		#region Public Properties

		public bool IsSingleton
		{
			get { return false; }
		}

		public Type ObjectType
		{
			get { return typeof(System.ServiceModel.Channels.Binding); }
		}

		public MexBindingProtocol Protocol { get; set; }

		#endregion Public Properties

		#region Public Methods

		public object GetObject()
		{
			System.ServiceModel.Channels.Binding returnValue = MexBindingResolver.Resolve(this.Protocol);
			return returnValue;
		}

		#endregion Public Methods
	}
}