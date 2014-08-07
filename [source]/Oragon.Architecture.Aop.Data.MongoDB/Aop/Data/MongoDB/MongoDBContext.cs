using Oragon.Architecture.Aop.Data.Abstractions;
using System.Collections.Generic;
using MongoDBDriver = MongoDB.Driver;

namespace Oragon.Architecture.Aop.Data.MongoDB
{
	public class MongoDBContext : AbstractContext<MongoDBContextAttribute>
	{
		#region Public Constructors

		public MongoDBContext(MongoDBContextAttribute contextAttribute, Stack<AbstractContext<MongoDBContextAttribute>> contextStack)
			: base(contextAttribute, contextStack)
		{
			this.Client = new MongoDBDriver.MongoClient(contextAttribute.MongoDBConnectionString.ConnectionString);
			this.Server = this.Client.GetServer();
		}

		#endregion Public Constructors

		#region Public Properties

		public MongoDBDriver.MongoClient Client { get; private set; }

		public MongoDBDriver.MongoServer Server { get; private set; }

		#endregion Public Properties

		#region Protected Methods

		protected override void DisposeContext()
		{
			base.DisposeContext();
		}

		protected override void DisposeFields()
		{
			this.Server = null;
			this.Client = null;
			base.DisposeFields();
		}

		#endregion Protected Methods
	}
}