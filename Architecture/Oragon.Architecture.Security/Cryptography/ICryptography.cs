using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Security.Cryptography
{
	public interface ICryptography
	{
		string Encrypt(string text);
		string Decrypt(string text);
	}
}
