using System;

namespace Oragon.Architecture.ApplicationHosting.Management.Storage
{
	public interface IStorageProvider
	{
		#region Public Properties

		string Description { get; }

		string Icon { get; }

		Guid ID { get; }

		string Name { get; }

		#endregion Public Properties
	}
}