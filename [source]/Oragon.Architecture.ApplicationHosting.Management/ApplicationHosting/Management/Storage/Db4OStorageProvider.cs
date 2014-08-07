using System;

namespace Oragon.Architecture.ApplicationHosting.Management.Storage
{
	public class Db4OStorageProvider : IStorageProvider
	{
		#region Naming and Identity

		public string Description
		{
			get { return "Storage applications packages with Db4Objects"; }
		}

		public string Icon
		{
			get { return "/dynRes/ApplicationHosting/Management/Images/icon-storage-db4o.fw.png"; }
		}

		public Guid ID
		{
			get { return Guid.Parse("{092231A8-75C4-4AC0-ACFD-D65345D8ED49}"); }
		}

		public string Name
		{
			get { return "Db4O Storage Provider"; }
		}

		#endregion Naming and Identity
	}
}