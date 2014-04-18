using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Security.DirectoryServices.Messages
{
	public class AuthenticationResult
	{
		public bool CredentialIsValid { get; set; }

		public User User { get; set; }

		
	}


	#region CodeGen




	public class AdvancedSearchFilter
	{
	}

	public class Context
	{
		public int ContextType { get; set; }
		public string Name { get; set; }
		public object Container { get; set; }
		public string UserName { get; set; }
		public int Options { get; set; }
		public string ConnectedServer { get; set; }
	}

	public class AccountDomainSid
	{
		public int BinaryLength { get; set; }
		public string Value { get; set; }
	}

	public class Sid
	{
		public int BinaryLength { get; set; }
		public AccountDomainSid AccountDomainSid { get; set; }
		public string Value { get; set; }
	}

	public class User
	{
		public bool PasswordExpired { get; set; }
		//public string GivenName { get; set; }
		//public string MiddleName { get; set; }
		//public string Surname { get; set; }
		//public string EmailAddress { get; set; }
		//public string VoiceTelephoneNumber { get; set; }
		//public object EmployeeId { get; set; }
		//public AdvancedSearchFilter AdvancedSearchFilter { get; set; }
		public bool Enabled { get; set; }
		//public DateTime? AccountLockoutTime { get; set; }
		//public DateTime? LastLogon { get; set; }
		//public object[] PermittedWorkstations { get; set; }
		//public object PermittedLogonTimes { get; set; }
		//public DateTime? AccountExpirationDate { get; set; }
		//public bool SmartcardLogonRequired { get; set; }
		//public bool DelegationPermitted { get; set; }
		//public int BadLogonCount { get; set; }
		//public string HomeDirectory { get; set; }
		//public string HomeDrive { get; set; }
		//public object ScriptPath { get; set; }
		//public DateTime? LastPasswordSet { get; set; }
		//public DateTime? LastBadPasswordAttempt { get; set; }
		//public bool PasswordNotRequired { get; set; }
		//public bool PasswordNeverExpires { get; set; }
		//public bool UserCannotChangePassword { get; set; }
		//public bool AllowReversiblePasswordEncryption { get; set; }
		//public Context Context { get; set; }
		//public int ContextType { get; set; }
		//public string Description { get; set; }
		//public string DisplayName { get; set; }
		//public string SamAccountName { get; set; }
		//public string UserPrincipalName { get; set; }
		//public Sid Sid { get; set; }
		//public string Guid { get; set; }
		//public string DistinguishedName { get; set; }
		//public string StructuralObjectClass { get; set; }
		//public string Name { get; set; }
		public bool UserMustChangePassword { get; set; }
		public DateTime LastPasswordSet { get; set; }
		public bool PasswordNeverExpire { get; set; }
	}



	#endregion

}
