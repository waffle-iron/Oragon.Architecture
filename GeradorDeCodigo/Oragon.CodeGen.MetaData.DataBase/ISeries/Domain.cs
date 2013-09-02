using System;
using System.Data;
using System.Data.OleDb;

namespace Oragon.CodeGen.MetaData.DataBase.ISeries
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomain))]
#endif 
	public class ISeriesDomain : Domain
	{
		public ISeriesDomain()
		{

		}
	}
}
