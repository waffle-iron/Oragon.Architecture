using Oragon.Architecture.Business;
using MongoDBDriver = MongoDB.Driver;

namespace Oragon.Architecture.Aop.Data.MongoDB
{
	public class MongoDBDataProcess : Oragon.Architecture.Aop.Data.Abstractions.AbstractDataProcess<MongoDBContext, MongoDBContextAttribute>
	{
	}

	public class MongoDBDataProcess<T> : Oragon.Architecture.Aop.Data.Abstractions.AbstractDataProcess<MongoDBContext, MongoDBContextAttribute>
		where T : Entity
	{
		#region Protected Properties

		protected virtual MongoDBDriver.IMongoCollection<T> Collection
		{
			get
			{
				return this
					.GetDataBase()
					.GetCollection<T>(this.CollectionName);
			}
		}

		protected string CollectionName { get; set; }

		protected string DataBaseName { get; set; }

		#endregion Protected Properties

		#region Protected Methods

		protected virtual MongoDBDriver.IMongoDatabase GetDataBase()
		{
			return this
					.ObjectContext
					.Client
					.GetDatabase(this.DataBaseName);
		}

		#endregion Protected Methods
	}
}