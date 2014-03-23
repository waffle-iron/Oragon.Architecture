using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NH = NHibernate;
using NHibernate.Linq;
using Spring.Objects.Factory.Attributes;
using Oragon.Architecture.Business;
using MongoDBDriver = MongoDB.Driver;

namespace Oragon.Architecture.AOP.Data.MongoDB
{

	public class MongoDBDataProcess : Oragon.Architecture.AOP.Data.Abstractions.AbstractDataProcess<MongoDBContext, MongoDBContextAttribute>
	{

	}

	public class MongoDBDataProcess<T> : Oragon.Architecture.AOP.Data.Abstractions.AbstractDataProcess<MongoDBContext, MongoDBContextAttribute>
		where T : Entity
	{
		protected string CollectionName { get; set; }
		
		protected string DataBaseName { get; set; }

		protected virtual MongoDBDriver.MongoDatabase GetDataBase()
		{
			return this
					.ObjectContext
					.Server
					.GetDatabase(this.DataBaseName);
		}

		protected virtual MongoDBDriver.MongoCollection<T> Collection
		{
			get
			{
				
				return this
					.GetDataBase()
					.GetCollection<T>(this.CollectionName);
			}
		}
	}

}
