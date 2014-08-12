using Oragon.Architecture.Extensions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Oragon.Architecture.Networking.Commom
{
	public class Message
	{
		#region Private Fields

		private List<MessageProperty> properties;

		#endregion Private Fields

		#region Public Constructors

		public Message()
		{
			this.properties = new List<MessageProperty>();
		}

		#endregion Public Constructors

		#region Public Methods

		public void AddProperty(MessageProperty property)
		{
			Contract.Requires(property.IsNotNull());
			Contract.Requires(property.Name.IsNotNullOrWhiteSpace());
			Contract.Requires(this.properties.NotContains(it => it.Name.ToLower() == property.Name.ToLower()));
			this.properties.Add(property);
		}

		public void AddProperty<T>(string name, T data)
		{
			Contract.Requires(name.IsNotNullOrWhiteSpace());
			Contract.Requires(this.properties.NotContains(it => it.Name.ToLower() == name.ToLower()));
			this.properties.Add(new MessageProperty<T>() { Data = data });
		}

		#endregion Public Methods
	}
}