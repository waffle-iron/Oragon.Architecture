using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Storage
{
	public interface IStorageProvider
	{
		Guid ID { get; }

		string Icon { get; }

		string Name { get; }

		string Description { get; }

	}
}
