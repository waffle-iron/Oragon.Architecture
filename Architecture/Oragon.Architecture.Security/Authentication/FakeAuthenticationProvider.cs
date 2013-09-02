using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Security.DirectoryServices;

namespace Oragon.Architecture.Security.Authentication
{
	public class FakeAuthenticationProvider : IAuthenticationProvider
	{
		public void Authenticate(string username, string password)
		{
			if (username.ToLower() != password.ToLower())
				throw UserNotFoundException.Build();
		}


		public void ChangePassword(string username, string password, string newPassword)
		{
			throw new NotImplementedException();
		}


		public void ChangePassword(string username, string newPassword)
		{
			throw new NotImplementedException();
		}


		public void UserMustChangePasswordatNextLogon(string userName)
		{
			throw new NotImplementedException();
		}
	}
}
