using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Oragon.Architecture.AOP.Data.NHibernate;
using Spring.Objects.Factory.Attributes;

namespace Oragon.Architecture.Data.Process
{
	public class QueryDataProcess<T> : NHDataProcess
		where T : Oragon.Architecture.Business.Entity
	{
		public bool UseCacheByDefault { get; set; }


		/// <summary>
		/// Obtém uma única instância de T com base no filtro informado. 
		/// </summary>
		/// <param name="predicate">Filtro a ser aplicado.</param>
		/// <returns>Primeira ocorrência de T que atenda ao filtro.</returns>
		public T GetFirstBy(Expression<Func<T, bool>> predicate)
		{
			return this.GetFirstBy(predicate, this.UseCacheByDefault);
		}

		/// <summary>
		/// Obtém uma única instância de T com base no filtro informado. 
		/// </summary>
		/// <param name="predicate">Filtro a ser aplicado.</param>
		/// <param name="cacheable">Informa para a consulta que pode esta pode ser cacheada</param>
		/// <returns>Primeira ocorrência de T que atenda ao filtro.</returns>
		public T GetFirstBy(Expression<Func<T, bool>> predicate, bool cacheable)
		{
			IQueryOver<T> queryOver = this.ObjectContext.Session.QueryOver<T>().Where(predicate);
			if (cacheable)
				queryOver.Cacheable().CacheMode(CacheMode.Normal);
			return queryOver.SingleOrDefault();
		}

		/// <summary>
		/// Obtém uma lista de instâncias de T com base no filtro informado.
		/// </summary>
		/// <param name="predicate">Filtro a ser aplicado.</param>
		/// <param name="cacheable">Informa para a consulta que pode esta pode ser cacheada</param>
		/// <param name="orderByExpression">Identifica o orderby a ser executado na consulta</param>
		/// <returns>Lista com ocorrência de T que atendem ao filtro.</returns>
		public IList<T> GetListBy(Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> orderByExpression = null, bool? cacheable = null)
		{
			if (cacheable == null)
				cacheable = this.UseCacheByDefault;
			NHibernate.IQueryOver<T, T> queryOver = this.ObjectContext.Session.QueryOver<T>();
			if (predicate != null)
				queryOver = queryOver.Where(predicate);
			if (orderByExpression != null)
				queryOver = queryOver.OrderBy(orderByExpression).Asc;
			if (cacheable.Value)
				queryOver.Cacheable().CacheMode(CacheMode.Normal);
			return queryOver.List();
		}

		/// <summary>
		/// Obtém uma lista com todas as instâncias de T.
		/// </summary>
		/// <returns>Lista com ocorrência de T.</returns>
		public IList<T> GetAll()
		{
			return this.GetListBy();
		}

	}
}
