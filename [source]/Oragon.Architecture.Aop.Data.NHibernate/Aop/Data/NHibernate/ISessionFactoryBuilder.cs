using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NH = NHibernate;

namespace Oragon.Architecture.Aop.Data.NHibernate
{
	public interface ISessionFactoryBuilder
	{
		NH.ISessionFactory BuildSessionFactory();
		System.Data.IsolationLevel DefaultIsolationLevel { get; }

		System.Data.IsolationLevel TransactionIsolationLevel { get; }

		NH.FlushMode DefaultFlushMode { get;  }

		NH.FlushMode TransactionFlushMode { get; }

		string ObjectContextKey { get; }
	}
}
