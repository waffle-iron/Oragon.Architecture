using Oragon.Architecture.Aop.Data.Abstractions;
using System.Collections.Generic;
using NH = NHibernate;

namespace Oragon.Architecture.Aop.Data.NHibernate
{
	public class NHContext : AbstractContext<NHContextAttribute>
	{
		#region Public Constructors

		public NHContext(NHContextAttribute contextAttribute, NH.IInterceptor interceptor, Stack<AbstractContext<NHContextAttribute>> contextStack)
			: base(contextAttribute, contextStack)
		{
			this.Session = this.BuildSession(interceptor);
			this.Transaction = this.BuildTransaction();
		}

		#endregion Public Constructors

		#region Public Properties

		public NH.ISession Session { get; private set; }

		#endregion Public Properties

		#region Protected Properties

		protected bool IsTransactional
		{
			get
			{
				return (this.ContextAttribute.IsTransactional.HasValue && this.ContextAttribute.IsTransactional.Value);
			}
		}

		#endregion Protected Properties

		#region Private Properties

		private NH.ITransaction Transaction { get; set; }

		#endregion Private Properties

		#region Public Methods

		public bool Complete()
		{
			bool returnValue = (this.IsTransactional && this.Transaction != null);
			if (returnValue)
				this.Transaction.Commit();
			return returnValue;
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		///     Constr�i uma sess�o NHibernate injetando interceptadores na sess�o de acordo com o estado definido na pr�pria configura��o do ObjectcontextAroundAdvice
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

		#endregion Private Methods

		#region Dispose Methods

		protected override void DisposeContext()
		{
			//TODO: Adicionado tratamento para a transaction. Na vers�o anterior n�o havia sido codificado, no entanto tamb�m n�o h� nenhum bug conhecido a respeito da falta desse c�digo.
			if (this.Transaction != null)
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

		#endregion Dispose Methods
	}
}