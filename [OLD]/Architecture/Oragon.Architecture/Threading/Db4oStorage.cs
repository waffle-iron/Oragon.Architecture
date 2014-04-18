using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Oragon.Architecture.Threading
{

	public class StrageItem
	{
		public string Name { get; set; }
		public object Value { get; set; }
	}


	public class Db4oStorage : IThreadStorage
	{
		string Server { get; set; }
		string FileName { get; set; }
		int Port { get; set; }
		string User { get; set; }
		string Password { get; set; }
		IObjectServer _server;
		IKeyParser KeyParser { get; set; }

		public void Initialize()
		{
			this._server = Db4oClientServer.OpenServer(this.FileName, this.Port);
			this._server.GrantAccess(this.User, this.Password);
		}

		~Db4oStorage()
		{
			this._server.Close();
			this._server.Dispose();
		}

		private string ResolveName(string name)
		{
			string newName = this.KeyParser.GetName(name);
			return newName;
		}

		public void FreeNamedDataSlot(string name)
		{
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				using (IObjectContainer client = Db4oClientServer.OpenClient(this.Server, this.Port, this.User, this.Password))
				{
					StrageItem strageItem = client.Query<StrageItem>().FirstOrDefault(it => it.Name == newName);
					if (strageItem != null)
						client.Delete(strageItem);
				}
			}
		}

		public object GetData(string name)
		{
			StrageItem strageItem = null;
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				using (IObjectContainer client = Db4oClientServer.OpenClient(this.Server, this.Port, this.User, this.Password))
				{
					strageItem = client.Query<StrageItem>().FirstOrDefault(it => it.Name == newName);
				}
			}
			if (strageItem == null)
				return null;
			else
				return strageItem.Value;
		}

		public void SetData(string name, object value)
		{
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				using (IObjectContainer client = Db4oClientServer.OpenClient(this.Server, this.Port, this.User, this.Password))
				{
					StrageItem strageItem = client.Query<StrageItem>().FirstOrDefault(it => it.Name == newName);
					if (strageItem == null)
						strageItem = new StrageItem() { Name = newName };
					strageItem.Value = value;
					client.Store(strageItem);
				}
			}
		}
	}
}
