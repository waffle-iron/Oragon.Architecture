using System;
using System.Data;
using System.Data.OleDb;

namespace Oragon.CodeGen.MetaData.DataBase.MySql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumns))]
#endif 
	public class MySqlResultColumns : ResultColumns
	{
		public MySqlResultColumns()
		{

		}
	}
}
