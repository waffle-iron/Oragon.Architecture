using System;
using System.Data;
using Npgsql;

namespace Oragon.CodeGen.MetaData.DataBase.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomain))]
#endif 
	public class PostgreSQLDomain : Domain
	{
		public PostgreSQLDomain()
		{

		}
	}
}
