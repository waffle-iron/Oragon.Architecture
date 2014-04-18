using System;
using System.Data.SQLite;

namespace Oragon.CodeGen.MetaData.DataBase.SQLite
{
	/// <summary>
	/// Summary description for ConnectionHelper.
	/// </summary>
	public class ConnectionHelper
	{
		public ConnectionHelper()
		{

		}
//
		static public SQLiteConnection CreateConnection(Oragon.CodeGen.MetaData.DataBase.dbRoot dbRoot)
		{
			SQLiteConnection cn = new SQLiteConnection(dbRoot.ConnectionString);
			cn.Open();
			//cn.ChangeDatabase(database);
			return cn;
		}
	}
}
