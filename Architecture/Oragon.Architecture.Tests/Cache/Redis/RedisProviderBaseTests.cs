using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oragon.Architecture.Cache.Redis;

namespace Oragon.Architecture.Tests.Cache.Redis
{
	[TestClass]
	public class RedisProviderBaseTests
	{

		[TestMethod]
		public void GetKeyTest1()
		{
			RedisProviderBase provider = new RedisProviderBase(null, string.Empty);
			string expectation = "A";
			string returnedValue = provider.GetKey("A");
			Assert.AreEqual(expectation, returnedValue);
		}

		[TestMethod]
		public void GetKeyTest2()
		{
			RedisProviderBase provider = new RedisProviderBase(null, "root");
			string expectation = "root:A";
			string returnedValue = provider.GetKey("A");
			Assert.AreEqual(expectation, returnedValue);
		}

		[TestMethod]
		public void GetKeyTest3()
		{
			RedisProviderBase provider = new RedisProviderBase(null, "root");
			string expectation = "root:A:B";
			string returnedValue = provider.GetKey("A:B");
			Assert.AreEqual(expectation, returnedValue);
		}

		[TestMethod]
		public void GetKeyTest4()
		{
			RedisProviderBase provider = new RedisProviderBase(null, "root");
			string expectation = "root:A:B";
			string returnedValue = provider.GetKey(":::A:B:::");
			Assert.AreEqual(expectation, returnedValue);
		}

		[TestMethod]
		public void GetKeyTest5()
		{
			RedisProviderBase provider = new RedisProviderBase(null, "");
			string expectation = "A:B";
			string returnedValue = provider.GetKey(":::A:B:::");
			Assert.AreEqual(expectation, returnedValue);
		}


		[TestMethod]
		public void GetKeyTest6()
		{
			RedisProviderBase provider = new RedisProviderBase(null, "root");
			string expectation = "root";
			string returnedValue = provider.GetKey();
			Assert.AreEqual(expectation, returnedValue);
		}
	}
}
