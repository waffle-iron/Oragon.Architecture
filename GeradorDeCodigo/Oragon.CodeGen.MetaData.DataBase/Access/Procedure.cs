using System;
using System.Data;
using System.Data.OleDb;

namespace Oragon.CodeGen.MetaData.DataBase.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class AccessProcedure : Procedure
	{
		public AccessProcedure()
		{

		}
	}
}
