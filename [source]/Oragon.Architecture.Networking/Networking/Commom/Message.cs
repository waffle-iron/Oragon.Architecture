using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using System.Diagnostics.Contracts;

namespace Oragon.Architecture.Networking.Commom
{
	public class Message
	{
		private List<MessageProperty> properties;

		public Message()
		{
			this.properties = new List<MessageProperty>();
		}

		public void AddProperty(MessageProperty property)
		{
			Contract.Ensures(property.IsNotNull());
			Contract.Ensures(property.Name.IsNotNullOrWhiteSpace());
			Contract.Ensures(this.properties.NotContains(it => it.Name.ToLower() == property.Name.ToLower()));
			this.properties.Add(property);
		}

		public void AddProperty<T>(string name, T data)
		{
			Contract.Ensures(name.IsNotNullOrWhiteSpace());
			Contract.Ensures(this.properties.NotContains(it => it.Name.ToLower() == name.ToLower()));
			this.properties.Add(new MessageProperty<T>() { Data = data });
		}
	}
}
