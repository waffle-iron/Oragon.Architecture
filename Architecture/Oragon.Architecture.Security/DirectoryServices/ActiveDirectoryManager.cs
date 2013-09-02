using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.IO;
using System.DirectoryServices.AccountManagement;
using Oragon.Architecture.Security.DirectoryServices.Messages;
using Oragon.Architecture.Security.Authentication;

namespace Oragon.Architecture.Security.DirectoryServices
{
	public class ActiveDirectoryManager
	{
		//const int SCRIPT = 0x0001;
		const int ACCOUNTDISABLE = 0x0002;
		//const int HOMEDIR_REQUIRED = 0x0008;
		//const int LOCKOUT = 0x0010;
		//const int PASSWD_NOTREQD = 0x0020;
		//const int PASSWD_CANT_CHANGE = 0x0040;
		//const int ENCRYPTED_TEXT_PWD_ALLOWED = 0x0080;
		//const int TEMP_DUPLICATE_ACCOUNT = 0x0100;
		//const int NORMAL_ACCOUNT = 0x0200;
		//const int INTERDOMAIN_TRUST_ACCOUNT = 0x0800;
		//const int WORKSTATION_TRUST_ACCOUNT = 0x1000;
		//const int SERVER_TRUST_ACCOUNT = 0x2000;
		const int DONT_EXPIRE_PASSWORD = 0x10000;
		//const int MNS_LOGON_ACCOUNT = 0x20000;
		//const int SMARTCARD_REQUIRED = 0x40000;
		//const int TRUSTED_FOR_DELEGATION = 0x80000;
		//const int NOT_DELEGATED = 0x100000;
		//const int USE_DES_KEY_ONLY = 0x200000;
		//const int DONT_REQ_PREAUTH = 0x400000;
		const int PASSWORD_EXPIRED = 0x800000;
		//const int TRUSTED_TO_AUTH_FOR_DELEGATION = 0x1000000;


		/// <summary>
		/// Representa o endereço do controlador de domínio. Injetado por IOC.
		/// </summary>
		protected virtual string Domain { get; set; }

		/// <summary>
		/// Representa as credenciais para administração de usuários no domínio. Injetado por IOC.
		/// </summary>
		protected virtual Credential MasterCredential { get; set; }

		internal AuthenticationResult Authenticate(string username, string password)
		{
			return this.Authenticate(new Credential() { Username = username, Password = password });
		}

		internal void ChangePassword(string userName, string passWord)
		{
			this.ChangePassword(new Credential() { Username = userName, Password = passWord });
		}

		internal void UserMustChangePasswordatNextLogon(string userName)
		{
			using (PrincipalContext ctx = this.GetContext())
			{
				UserPrincipal userPrincipal = this.FindUser(ctx, userName);
				if (userPrincipal != null)
				{
					DirectoryEntry directoryEntry = (DirectoryEntry)userPrincipal.GetUnderlyingObject();
					this.SetUserMustChangePassword(directoryEntry, true);
				}
				else
					throw new UserNotFoundException();
			}
		}

		private PrincipalContext GetContext()
		{
			PrincipalContext ctx = new PrincipalContext(ContextType.Domain, this.Domain, this.MasterCredential.Username, this.MasterCredential.Password);
			return ctx;
		}

		private UserPrincipal FindUser(PrincipalContext ctx, string username)
		{
			UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, username);
			return user;
		}

		private AuthenticationResult Authenticate(Credential userCredential)
		{
			if (userCredential == null)
				throw new ArgumentNullException("userCredential");
			if (string.IsNullOrWhiteSpace(userCredential.Username))
				throw new ArgumentNullException("userCredential.Username");
			if (string.IsNullOrWhiteSpace(userCredential.Password))
				throw new ArgumentNullException("userCredential.Password");

			AuthenticationResult returnValue = new AuthenticationResult();
			using (PrincipalContext ctx = this.GetContext())
			{
				UserPrincipal userPrincipal = this.FindUser(ctx, userCredential.Username);
				if (userPrincipal != null)
				{
					returnValue.User = this.Convert(userPrincipal);
					returnValue.CredentialIsValid = this.ValidateCredentials(ctx, userPrincipal, returnValue.User, userCredential);
				}
				else
					returnValue.CredentialIsValid = false;
			}
			return returnValue;
		}

		private void ChangePassword(Credential userCredential)
		{
			using (PrincipalContext ctx = this.GetContext())
			{
				UserPrincipal userPrincipal = this.FindUser(ctx, userCredential.Username);
				if (userPrincipal != null)
				{
					DirectoryEntry directoryEntry = (DirectoryEntry)userPrincipal.GetUnderlyingObject();
					userPrincipal.SetPassword(userCredential.Password);
					userPrincipal.Save();
					this.SetUserMustChangePassword(directoryEntry, false);
				}
				else
					throw new UserNotFoundException();
			}
		}

		private bool ValidateCredentials(PrincipalContext ctx, UserPrincipal userPrincipal, User user, Credential userCredential)
		{
			bool setUserMustChangePassword = user.UserMustChangePassword;
			DirectoryEntry directoryEntry = (DirectoryEntry)userPrincipal.GetUnderlyingObject();
			if (setUserMustChangePassword)
				this.SetUserMustChangePassword(directoryEntry, false);
			bool returnValue = ctx.ValidateCredentials(userCredential.Username, userCredential.Password, ContextOptions.Negotiate);
			if (setUserMustChangePassword)
				this.SetUserMustChangePassword(directoryEntry, true);
			return returnValue;
		}

		private User Convert(UserPrincipal user)
		{
			DirectoryEntry entry = (DirectoryEntry)user.GetUnderlyingObject();
			entry.RefreshCache();
			User returnValue = new User()
			{
				UserMustChangePassword = this.ExtractUserMustChangePassword(entry),
				PasswordExpired = this.ExtractPasswordExpired(entry),
				LastPasswordSet = this.ExtracLastChangePassword(entry),
				PasswordNeverExpire = this.ExtractPasswordNeverExpire(entry),
				Enabled = this.ExtractUserEnable(entry)
			};
			return returnValue;
		}

		private int UserAccountControl(DirectoryEntry entry)
		{
			int userAccountControl = System.Convert.ToInt32(entry.Properties["userAccountControl"].Value, System.Globalization.CultureInfo.InvariantCulture);
			return userAccountControl;
		}

		#region Extração de Dados do AD

		private System.Int64 ExtractPwdLastSet(DirectoryEntry entry)
		{
			ActiveDs.IADsLargeInteger int64Val = (ActiveDs.IADsLargeInteger)entry.Properties["pwdLastSet"].Value;
			System.Int64 largeInt = int64Val.HighPart * 0x100000000 + int64Val.LowPart;
			return largeInt;
		}

		private bool ExtractUserMustChangePassword(DirectoryEntry entry)
		{
			System.Int64 largeInt = this.ExtractPwdLastSet(entry);
			bool returnValue = (largeInt == 0);
			return returnValue;
		}

		private DateTime ExtracLastChangePassword(DirectoryEntry entry)
		{
			System.Int64 largeInt = this.ExtractPwdLastSet(entry);
			DateTime pwdLastSet = DateTime.FromFileTime(largeInt);
			return pwdLastSet;
		}

		private void SetUserMustChangePassword(DirectoryEntry entry, bool value)
		{
			entry.Properties["pwdLastSet"].Value = value ? 0 : -1;
			entry.CommitChanges();
			entry.RefreshCache();
		}

		private bool ExtractUserEnable(DirectoryEntry entry)
		{
			int userAccountControl = this.UserAccountControl(entry);
			bool returnValue = !((userAccountControl & ActiveDirectoryManager.ACCOUNTDISABLE) == ActiveDirectoryManager.ACCOUNTDISABLE);
			return returnValue;
		}

		private bool ExtractPasswordExpired(DirectoryEntry entry)
		{
			int userAccountControl = this.UserAccountControl(entry);
			bool returnValue = ((userAccountControl & ActiveDirectoryManager.PASSWORD_EXPIRED) == ActiveDirectoryManager.PASSWORD_EXPIRED);
			return returnValue;
		}

		private bool ExtractPasswordNeverExpire(DirectoryEntry entry)
		{
			int userAccountControl = this.UserAccountControl(entry);
			bool returnValue = ((userAccountControl & ActiveDirectoryManager.DONT_EXPIRE_PASSWORD) == ActiveDirectoryManager.DONT_EXPIRE_PASSWORD);
			return returnValue;
		}

		#endregion

		
	}
}
