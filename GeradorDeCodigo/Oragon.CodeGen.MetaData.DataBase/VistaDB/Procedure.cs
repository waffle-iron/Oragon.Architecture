using System;
using System.Data;

namespace Oragon.CodeGen.MetaData.DataBase.VistaDB
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class VistaDBProcedure : Procedure
	{
		public VistaDBProcedure()
		{

		}
	}
}
