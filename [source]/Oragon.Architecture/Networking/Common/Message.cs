using Oragon.Architecture.Extensions;
using System.Collections.Generic;
using FluentAssertions;

namespace Oragon.Architecture.Networking.Common
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
			property.Should().NotBeNull();
			property.Name.Should().NotBeNullOrWhiteSpace();
			this.properties.NotContains(it => it.Name.ToLower() == property.Name.ToLower()).Should().BeTrue();
			this.properties.Add(property);
		}

		public void AddProperty<T>(string name, T data)
		{
			name.Should().NotBeNullOrWhiteSpace();
			this.properties.NotContains(it => it.Name.ToLower() == name.ToLower()).Should().BeTrue();
			this.properties.Add(new MessageProperty<T>() { Data = data });
		}

		#endregion Public Methods
	}
}