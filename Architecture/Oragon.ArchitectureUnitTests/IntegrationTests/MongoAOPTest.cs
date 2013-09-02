using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spring.Testing.Microsoft;
using Oragon.Architecture.AOP.Data;
using Oragon.Architecture.AOP.Data.MongoDB;

namespace Oragon.ArchitectureUnitTests.IntegrationTests
{
	[TestClass]
	public class MongoAOPTest : TestBase
	{
		public IMongoAOPTestService MongoAOPTestService { get; set; }

		[TestMethod]
		public void TestMethod1()
		{
			bool serviceResult = MongoAOPTestService.ExistsDataBaseDDEX();
			Assert.AreEqual(serviceResult, true);

		}
	}


	public interface IMongoAOPTestService
	{
		bool ExistsDataBaseDDEX();
	}
	public class MongoAOPTestService : Oragon.Architecture.AOP.Data.Abstractions.AbstractDataProcess<MongoDBContext, MongoDBContextAttribute>, IMongoAOPTestService
	{
		[MongoDBContextAttribute("MongoDB01")]
		public bool ExistsDataBaseDDEX()
		{
			bool returnValue = this.ObjectContext.Database.CollectionExists("ddex");
			return returnValue;
		}
	}


}
