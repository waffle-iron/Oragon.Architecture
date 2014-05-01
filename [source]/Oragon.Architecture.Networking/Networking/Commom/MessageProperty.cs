using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Networking.Commom
{
	public abstract class MessageProperty
	{
		public string Name { get; set; }
	}

	public class MessageProperty<T> : MessageProperty
	{
		public T Data { get; set; }
	}
}
