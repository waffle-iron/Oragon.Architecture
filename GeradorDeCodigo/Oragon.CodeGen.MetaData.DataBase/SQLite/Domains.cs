using System;
using System.Data;
using System.Data.SQLite;

namespace Oragon.CodeGen.MetaData.DataBase.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomains))]
#endif 
	public class SQLiteDomains : Domains
	{
		public SQLiteDomains()
		{

		}
	}
}
