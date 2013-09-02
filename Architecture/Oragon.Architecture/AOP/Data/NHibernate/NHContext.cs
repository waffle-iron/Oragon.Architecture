using Oragon.Architecture.AOP.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NH = NHibernate;


namespace Oragon.Architecture.AOP.Data.NHibernate
{
	public class NHContext : AbstractContext<NHContextAttribute> 
	{
		public NH.ISession Session { get; private set; }
		private NH.ITransaction Transaction { get; set; }

		public NHContext(NHContextAttribute contextAttribute, NH.IInterceptor interceptor, Stack<AbstractContext<NHContextAttribute>> contextStack)
			: base(contextAttribute, contextStack)
		{
			this.Session = this.BuildSession(interceptor);
			this.Transaction = this.BuildTransaction();
		}

		
		protected bool IsTransactional
		{
			get
			{
				return (this.ContextAttribute.IsTransactional.HasValue && this.ContextAttribute.IsTransactional.Value);
			}
		}


		/// <summary>
		/// Constrói uma sessão NHibernate injetando interceptadores na sessão de acordo com o estado definido na própria configuração do ObjectcontextAroundAdvice
		/// </summary>
		/// <param name="sessionFactory"></param>
		/// <returns></returns>
		private NH.ISession BuildSession(NH.IInterceptor interceptor)
		{
			NH.ISessionFactory sessionFactory = this.ContextAttribute.SessionFactoryBuilder.BuildSessionFactory();
			NH.ISession session = null;
			if (interceptor != null)
				session = sessionFactory.OpenSession(interceptor);
			else
				session = sessionFactory.OpenSession();

			if (this.IsTransactional)
				session.FlushMode = this.ContextAttribute.SessionFactoryBuilder.TransactionFlushMode;
			else
				session.FlushMode = this.ContextAttribute.SessionFactoryBuilder.DefaultFlushMode;
			return session;
		}

		private NH.ITransaction BuildTransaction()
		{
			NH.ITransaction returnValue = null;
			if (this.IsTransactional)
			{
				returnValue = this.Session.BeginTransaction(this.ContextAttribute.SessionFactoryBuilder.TransactionIsolationLevel);
			}
			return returnValue;
		}


		public bool Complete()
		{
			bool returnValue = (this.IsTransactional && this.Transaction != null);
			if (returnValue)
				this.Transaction.Commit();
			return returnValue;
		}

		#region Dispose Methods

		protected override void DisposeContext()
		{
			//TODO: Adicionado tratamento para a transaction. Na versão anterior não havia sido codificado, no entanto também não há nenhum bug conhecido a respeito da falta desse código.
			if(this.Transaction != null)
				this.Transaction.Dispose();

			if (this.Session != null)
				this.Session.Dispose();

			base.DisposeContext();
		}
		

		protected override void DisposeFields()
		{
			this.Session = null;
			this.Transaction = null;
			base.DisposeFields();
		}

		#endregion
	}
}
