using Oragon.Architecture.Data.ConnectionStrings;
using Spring.Objects.Factory.Attributes;
using Spring.Threading;
using System;
using System.Collections.Generic;
using NH = NHibernate;

namespace Oragon.Architecture.Aop.Data.NHibernate
{
	/// <summary>
	///     Respons�vel por inicializar a configura��io do NHibernate e disponibilizar um SessionFactory pra a aplica��o
	/// </summary>
	public abstract class AbstractSessionFactoryBuilder : ISessionFactoryBuilder
	{
		#region Inje��o de Depend�ncia

		/// <summary>
		///     Define o default Batch Size
		/// </summary>
		[Required]
		public int BatchSize { get; set; }

		/// <summary>
		///     Define o timeout padr�o para a execu��o de comandos
		/// </summary>
		[Required]
		public int CommandTimeout { get; set; }

		/// <summary>
		///     Identifica qual a chave da conex�o
		/// </summary>
		[Required]
		public IConnectionStringDiscoverer ConnectionStringDiscoverer { get; set; }

		[Required]
		public NH.FlushMode DefaultFlushMode { get; private set; }

		/// <summary>
		///     Define o nivel de isolamento da sess�o
		/// </summary>
		[Required]
		public System.Data.IsolationLevel DefaultIsolationLevel { get; private set; }

		[Required]
		public bool EnabledDiagnostics { get; set; }

		/// <summary>
		///     Define a profundidade m�xima para o preenchimento autom�tico do mecanismo de persist�ncia.
		/// </summary>
		[Required]
		public int MaxFetchDepth { get; set; }

		/// <summary>
		///     Define uma chave para acesso ao banco
		/// </summary>
		[Required]
		public string ObjectContextKey { get; private set; }

		[Required]
		public NH.FlushMode TransactionFlushMode { get; private set; }

		[Required]
		public System.Data.IsolationLevel TransactionIsolationLevel { get; private set; }

		/// <summary>
		///     Identifica tipos contidos em
		/// </summary>
		[Required]
		public List<string> TypeNames { get; set; }

		#endregion Inje��o de Depend�ncia

		#region Instance State

		private Semaphore semaphore = new Semaphore(1);
		private volatile NH.ISessionFactory sessionFactory;

		#endregion Instance State

		#region Public Constructors

		protected AbstractSessionFactoryBuilder()
		{
		}

		#endregion Public Constructors

		#region M�todos P�blicos

		public NH.ISessionFactory BuildSessionFactory()
		{
			if (this.sessionFactory == null)
			{
				semaphore.Acquire();
				if (this.sessionFactory == null)//Reteste.. outra thread pode ter feito o preenchimento do campo antes da libera��o do sem�foto.
				{
					try
					{
						this.sessionFactory = this.BuildSessionFactoryInternal();
					}
					catch (Exception)
					{
						throw;
					}
					finally
					{
						semaphore.Release();
					}
				}
				else
					semaphore.Release();
			}
			return this.sessionFactory;
		}

		#endregion M�todos P�blicos

		#region M�todos Privados

		/// <summary>
		///     Principal m�todo privado, realiza a cria��o do SessionFactory e este n�o deve ser criado novamente at� que o dom�nio de aplica��o seja finalizado.
		/// </summary>
		/// <returns></returns>
		protected abstract NH.ISessionFactory BuildSessionFactoryInternal();

		#endregion M�todos Privados
	}
}