using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Business;

namespace Oragon.Architecture.Security.DirectoryServices
{

	
	[Serializable]
	public class AuthenticationException : BusinessException
	{
		public AuthenticationException() { }
		public AuthenticationException(string message) : base(message) { }
		public AuthenticationException(string message, Exception inner) : base(message, inner) { }
		protected AuthenticationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}


	[Serializable]
	public class UserNotFoundException : AuthenticationException
	{
		public UserNotFoundException() { }
		public UserNotFoundException(string message) : base(message) { }
		public UserNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected UserNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public static UserNotFoundException Build()
		{
			return new UserNotFoundException("Usu�rio e/ou senha inv�lidos.");
		}

		public static UserNotFoundException Build(Exception ex)
		{
			return new UserNotFoundException("Usu�rio e/ou senha inv�lidos.", ex);
		}
	}

	[Serializable]
	public class PasswordPolicyException : AuthenticationException
	{
		public PasswordPolicyException () { }
		public PasswordPolicyException (string message) : base(message) { }
		public PasswordPolicyException (string message, Exception inner) : base(message, inner) { }
		protected PasswordPolicyException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public static PasswordPolicyException Build(Exception ex)
		{
			return new PasswordPolicyException(
@"Os requisitos m�nimos de seguran�a para a senha de rede Oragon s�o:
<br> * A nova senha deve ter no m�nimo 6 (seis) caracteres
<br> * Deve conter caracteres num�ricos e alfab�ticos (podem ser mai�sculos,min�sculos ou combinados)
<br> * Deve conter pelo menos um caracter especial (! # @?,. Etc)
<br> * A senha n�o pode conter o seu login, seja na totalidade ou apenas parte dele
<br> * A senha n�o pode ser igual �s 5 �ltimas senhas utilizadas", ex);
		}

	}


	[Serializable]
	public class PasswordExpiredException : AuthenticationException
	{
		public PasswordExpiredException() { }
		public PasswordExpiredException(string message) : base(message) { }
		public PasswordExpiredException(string message, Exception inner) : base(message, inner) { }
		protected PasswordExpiredException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public static PasswordExpiredException BuildExpired()
		{
			return new PasswordExpiredException("Sua senha expirou, ser� necess�rio trocar sua senha antes de acessar o sistema.");
		}

		public static PasswordExpiredException BuildMustChangePassword()
		{
			return new PasswordExpiredException("Ser� necess�rio trocar sua senha antes de acessar o sistema.");
		}
	}


	[Serializable]
	public class UserBlockedException : AuthenticationException
	{
		public UserBlockedException() { }
		public UserBlockedException(string message) : base(message) { }
		public UserBlockedException(string message, Exception inner) : base(message, inner) { }
		protected UserBlockedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public static UserBlockedException Build()
		{
			return new UserBlockedException("Usu�rio bloqueado! Entre em contato com o seu gestor ou respons�vel pela TI.");
		}
	}


}
