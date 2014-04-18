using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Oragon.Architecture.Data
{
	public static class IQueryExtensions
	{
		public static IQuery SetInt32(this IQuery query, string name, int? val)
		{
			if (val.HasValue)
			{
				return query.SetInt32(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Int32);
			}
		}

		public static IQuery SetDateTime(this IQuery query, string name, DateTime? val)
		{
			if (val.HasValue)
			{
				return query.SetDateTime(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.DateTime);
			}
		}

		public static IQuery SetDecimal(this IQuery query, string name, Decimal? val)
		{
			if (val.HasValue)
			{
				return query.SetDecimal(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Decimal);
			}
		}
	}
}
