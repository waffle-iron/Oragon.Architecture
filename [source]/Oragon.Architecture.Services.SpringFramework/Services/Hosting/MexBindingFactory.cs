using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Services.Hosting
{
	public class MexBindingFactory : IFactoryObject
	{
		public MexBindingProtocol Protocol { get; set; }

		public object GetObject()
		{
			System.ServiceModel.Channels.Binding returnValue = MexBindingResolver.Resolve(this.Protocol);
			return returnValue;
		}

		public bool IsSingleton
		{
			get { return false; }
		}

		public Type ObjectType
		{
			get { return typeof(System.ServiceModel.Channels.Binding); }
		}
	}
}
