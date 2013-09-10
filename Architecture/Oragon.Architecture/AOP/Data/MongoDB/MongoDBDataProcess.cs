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
		public string CollectionName { get; set; }

		public MongoDBDriver.MongoCollection<T> Collection
		{
			get
			{
				return this.ObjectContext.Database.GetCollection<T>(this.CollectionName);
			}
		}
	}

}
