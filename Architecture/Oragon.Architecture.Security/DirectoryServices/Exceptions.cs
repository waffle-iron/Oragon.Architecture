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
			return new UserNotFoundException("Usuário e/ou senha inválidos.");
		}

		public static UserNotFoundException Build(Exception ex)
		{
			return new UserNotFoundException("Usuário e/ou senha inválidos.", ex);
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
@"Os requisitos mínimos de segurança para a senha de rede Oragon são:
<br> * A nova senha deve ter no mínimo 6 (seis) caracteres
<br> * Deve conter caracteres numéricos e alfabéticos (podem ser maiúsculos,minúsculos ou combinados)
<br> * Deve conter pelo menos um caracter especial (! # @?,. Etc)
<br> * A senha não pode conter o seu login, seja na totalidade ou apenas parte dele
<br> * A senha não pode ser igual às 5 últimas senhas utilizadas", ex);
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
			return new PasswordExpiredException("Sua senha expirou, será necessário trocar sua senha antes de acessar o sistema.");
		}

		public static PasswordExpiredException BuildMustChangePassword()
		{
			return new PasswordExpiredException("Será necessário trocar sua senha antes de acessar o sistema.");
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
			return new UserBlockedException("Usuário bloqueado! Entre em contato com o seu gestor ou responsável pela TI.");
		}
	}


}
