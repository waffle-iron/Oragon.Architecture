using System;
using System.Data;
using Npgsql;

namespace Oragon.CodeGen.MetaData.DataBase.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomains))]
#endif 
	public class PostgreSQLDomains : Domains
	{
		public PostgreSQLDomains()
		{

		}
	}
}
