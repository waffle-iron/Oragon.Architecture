using System;
using System.DirectoryServices;
using System.IO;

namespace Oragon.Architecture.Security.DirectoryServices
{
	public class ActiveDirectoryUser
	{
		private SearchResult queryResult;

		public ActiveDirectoryUser(SearchResult queryResult)
		{
			if (queryResult == null)
				throw new ArgumentNullException();
			if (queryResult.Properties["objectclass"].Contains("user") == false)
				throw new InvalidOperationException("Objeto não é usuário");
			this.queryResult = queryResult;
		}

		public int BadPasswordCount { get { return (int)this.queryResult.Properties["badpwdcount"][0]; } }
		public string CN { get { return (string)this.queryResult.Properties["cn"][0]; } }
		public string AccountName { get { return (string)this.queryResult.Properties["samaccountname"][0]; } }
		public string Homedrive { get { return (string)this.queryResult.Properties["homedrive"][0]; } }
		public string DisplayName { get { return (string)this.queryResult.Properties["displayname"][0]; } }
		public string LastLogon { get { return (string)this.queryResult.Properties["lastlogon"][0]; } }
		public string HomeDirectory { get { return (string)this.queryResult.Properties["homedirectory"][0]; } }
		public int LogonCount { get { return (int)this.queryResult.Properties["logoncount"][0]; } }
		public string BadPasswordTime { get { return (string)this.queryResult.Properties["badpasswordtime"][0]; } }
		public string lastlogontimestamp { get { return (string)this.queryResult.Properties["lastlogontimestamp"][0]; } }
		public string Name { get { return (string)this.queryResult.Properties["name"][0]; } }
		public string ADSPath { get { return (string)this.queryResult.Properties["adspath"][0]; } }
		public string SN { get { return (string)this.queryResult.Properties["sn"][0]; } }
		public string Mail { get { return (string)this.queryResult.Properties["mail"][0]; } }
		public DateTime AccountExpires { get { return new DateTime((long)this.queryResult.Properties["accountexpires"][0]); } }
		public string PasswordLastSet { get { return (string)this.queryResult.Properties["pwdlastset"][0]; } }


		public string Path { get { return this.queryResult.Path; } }

		internal void ToFile(string fileName)
		{
			StringWriter writer = new StringWriter();
			if (queryResult != null)
			{
				//if (queryResult.Path.Contains("lfaria"))
				{
					writer.WriteLine(queryResult.Path);
					foreach (System.Collections.DictionaryEntry entry in queryResult.Properties)
					{
						writer.Write("\t{0} = ", entry.Key);
						foreach (object childEntry in entry.Value as ResultPropertyValueCollection)
						{
							writer.Write("{0},", childEntry);
						}
						writer.WriteLine(string.Empty);
					}
				}
			}
			File.WriteAllText(fileName, writer.ToString());
		}
	}
}
