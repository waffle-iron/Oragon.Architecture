using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Security.DirectoryServices;
using Oragon.Architecture.Security.DirectoryServices.Messages;

namespace Oragon.Architecture.Security.Authentication
{
	public class ActiveDirectoryAuthenticationProvider : IAuthenticationProvider
	{
		int PasswordMaxAge { get; set; }
		ActiveDirectoryManager ActiveDirectoryManager { get; set; }

		public void Authenticate(string username, string password)
		{
			AuthenticationResult authResult = this.ActiveDirectoryManager.Authenticate(username: username, password: password);
			if (authResult.User == null) 
				throw UserNotFoundException.Build();

			else if (
						(authResult.User.PasswordNeverExpire == false)
						&&
						(authResult.User.LastPasswordSet != null)
						&& 
						(authResult.User.LastPasswordSet.AddDays(this.PasswordMaxAge).Date <= DateTime.Now)
					)
				throw PasswordExpiredException.BuildExpired();

			else if (authResult.CredentialIsValid == false)
				throw UserNotFoundException.Build();

			else if (authResult.User.PasswordExpired)
				throw PasswordExpiredException.BuildExpired();
			
			else if (authResult.User.Enabled == false)
				throw UserBlockedException.Build();
			
			else if (authResult.User.UserMustChangePassword)
				throw PasswordExpiredException.BuildMustChangePassword();
		}

		public void ChangePassword(string username, string password, string newPassword)
		{
			try
			{
				this.Authenticate(username, password);
				this.ChangePassword(username, newPassword);
			}
			catch (PasswordExpiredException)
			{
				try
				{
					this.ChangePassword(username, newPassword);
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("0x800708C5"))
						throw PasswordPolicyException.Build(ex);
					else
						throw;
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("0x800708C5"))
					throw PasswordPolicyException.Build(ex);
				else
					throw;
			}
		}

		public void ChangePassword(string userName, string newPassWord)
		{
			this.ActiveDirectoryManager.ChangePassword(userName: userName, passWord: newPassWord);
		}

		public void UserMustChangePasswordatNextLogon(string userName)
		{
			this.ActiveDirectoryManager.UserMustChangePasswordatNextLogon(userName: userName);
		}
	}
}
