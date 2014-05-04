using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Storage
{
	public class Db4OStorageProvider : IStorageProvider
	{
		#region Naming and Identity

		public Guid ID
		{
			get { return Guid.Parse("{092231A8-75C4-4AC0-ACFD-D65345D8ED49}"); }
		}

		public string Icon
		{
			get { return "/dynRes/ApplicationHosting/Management/Images/icon-storage-db4o.fw.png"; }
		}

		public string Name
		{
			get { return "Db4O Storage Provider"; }
		}

		public string Description
		{
			get { return "Storage applications packages with Db4Objects"; }
		}

		#endregion


	}
}
