using System;
using System.Collections.Generic;
using System.Configuration;
using Spring.Threading;
using NH = NHibernate;
using Oragon.Architecture.Data.ConnectionStrings;
using Spring.Objects.Factory.Attributes;

namespace Oragon.Architecture.Aop.Data.NHibernate
{
	/// <summary>
	/// Responsável por inicializar a configuraçãio do NHibernate e disponibilizar um SessionFactory pra a aplicação
	/// </summary>
	public abstract class AbstractSessionFactoryBuilder : ISessionFactoryBuilder
	{
		#region Injeção de Dependência

		/// <summary>
		/// Identifica qual a chave da conexão
		/// </summary>
		[Required]
		public IConStrConfigDiscovery ConStrConfigDiscovery { get; set; }

		/// <summary>
		/// Identifica tipos contidos em 
		/// </summary>
		[Required]
		public List<string> TypeNames { get; set; }

		/// <summary>
		/// Define a profundidade máxima para o preenchimento automático do mecanismo de persistência.
		/// </summary>
		[Required]
		public int MaxFetchDepth { get; set; }

		/// <summary>
		/// Define o nivel de isolamento da sessão
		/// </summary>
		[Required]
		public System.Data.IsolationLevel DefaultIsolationLevel { get; private set; }

		[Required]
		public System.Data.IsolationLevel TransactionIsolationLevel { get; private set; }

		[Required]
		public NH.FlushMode DefaultFlushMode { get; private set; }

		[Required]
		public NH.FlushMode TransactionFlushMode { get; private set; }


		/// <summary>
		/// Define o timeout padrão para a execução de comandos
		/// </summary>
		[Required]
		public int CommandTimeout { get; set; }

		/// <summary>
		/// Define o default Batch Size
		/// </summary>
		[Required]
		public int BatchSize { get; set; }

		/// <summary>
		/// Define uma chave para acesso ao banco
		/// </summary>
		[Required]
		public string ObjectContextKey { get; private set; }

		[Required]
		public bool EnabledDiagnostics { get; set; }

		#endregion

		#region Instance State

		private volatile NH.ISessionFactory sessionFactory;

		private Semaphore semaphore = new Semaphore(1);

		#endregion

		public AbstractSessionFactoryBuilder()
		{
		}

		#region Métodos Públicos

		public NH.ISessionFactory BuildSessionFactory()
		{
			if (this.sessionFactory == null)
			{
				semaphore.Acquire();
				if (this.sessionFactory == null)//Reteste.. outra thread pode ter feito o preenchimento do campo antes da liberação do semáfoto.
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

		#endregion

		#region Métodos Privados

		/// <summary>
		/// Principal método privado, realiza a criação do SessionFactory e este não deve ser criado novamente até que o domínio de aplicação seja finalizado.
		/// </summary>
		/// <returns></returns>
		protected abstract NH.ISessionFactory BuildSessionFactoryInternal();
	

		#endregion
	}
}
