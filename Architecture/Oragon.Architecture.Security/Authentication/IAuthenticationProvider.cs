using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Security.Authentication
{
	public interface IAuthenticationProvider
	{
		void Authenticate(string username, string password);

		void ChangePassword(string username, string password, string newPassword);

		void ChangePassword(string username, string newPassword);

		void UserMustChangePasswordatNextLogon(string userName);
	}
}
