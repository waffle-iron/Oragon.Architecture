using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Spring.Objects.Factory.Attributes;

namespace Oragon.Architecture.Aop.Data.NHibernate
{
	public abstract class NHPersistenceDataProcessBase<T> : NHDataProcess
		where T : Oragon.Architecture.Business.Entity
	{
		
		/// <summary>
		/// Realiza a operação de insert dos dados de uma entidade no banco de dados
		/// </summary>
		/// <param name="entity">Entidade a ser persistida</param>
		public virtual void Save(T entity)
		{
			this.ObjectContext.Session.Save(entity);
		}

		/// <summary>
		/// Tenta realizar um insert ou update para garantir o armazenamento do dado no banco
		/// </summary>
		/// <param name="entity">Entidade a ser persistida</param>
		public virtual void SaveOrUpdate(T entity)
		{
			this.ObjectContext.Session.SaveOrUpdate(entity);
		}

		/// <summary>
		/// Realiza uma operãção de update no banco
		/// </summary>
		/// <param name="entity">Entidade a ser persistida</param>
		public virtual void Update(T entity)
		{
			this.ObjectContext.Session.Update(entity);
		}

		/// <summary>
		/// Realiza a exclusão da entidade do banco
		/// </summary>
		/// <param name="entity">Entidade a ser persistida</param>
		public virtual void Delete(T entity)
		{
			this.ObjectContext.Session.Delete(entity);
		}

	}
}
