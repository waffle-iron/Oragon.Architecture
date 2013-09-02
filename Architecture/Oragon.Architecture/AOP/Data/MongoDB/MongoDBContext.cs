using Oragon.Architecture.AOP.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDBDriver = MongoDB.Driver;

namespace Oragon.Architecture.AOP.Data.MongoDB
{
	public class MongoDBContext : AbstractContext<MongoDBContextAttribute>
	{

		public MongoDBContext(MongoDBContextAttribute contextAttribute, Stack<AbstractContext<MongoDBContextAttribute>> contextStack)
			: base(contextAttribute, contextStack)
		{

			this.Client = new MongoDBDriver.MongoClient(contextAttribute.MongoDBConnectionString.ConnectionString);
			this.Server = this.Client.GetServer();
			this.Database = this.Server.GetDatabase(contextAttribute.MongoDBConnectionString.Database);
		}

		public MongoDBDriver.MongoClient Client { get; private set; }
		public MongoDBDriver.MongoServer Server { get; private set; }
		public MongoDBDriver.MongoDatabase Database { get; private set; }



		protected override void DisposeContext()
		{
			base.DisposeContext();
		}

		protected override void DisposeFields()
		{
			this.Database = null;
			this.Server = null;
			this.Client = null;
			base.DisposeFields();
		}

	}
}
