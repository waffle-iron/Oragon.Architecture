using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oragon.Architecture.Cache.Redis;
using ServiceStack.Redis;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.Tests.Cache.Redis
{
	[TestClass]
	public class RedisClientForNHibernateTests
	{
		private IRedisClient BuildRedisClient(string serverName, string auth, int? port)
		{
			string redisConnectionString = string.Empty;
			if (auth.IsNotNullOrWhiteSpace())
				redisConnectionString = string.Concat(redisConnectionString, auth, "@");

			redisConnectionString = string.Concat(redisConnectionString, serverName);

			if (port.IsNotNull())
				redisConnectionString = string.Concat(redisConnectionString, ":", port.ToString());

			BasicRedisClientManager manager = new BasicRedisClientManager(redisConnectionString);
			return manager.GetClient();
		}

		private RedisClientForNHibernate BuildClientForNHibernate(string serverName, string auth, int? port, string isolationKey)
		{
			IRedisClient redisClient = this.BuildRedisClient(serverName: serverName, auth: auth, port: port);
			return new RedisClientForNHibernate(redisClient, isolationKey)
			{
				RegionName = "defaultRegion",
				CacheLifeTime = new TimeSpan(0, 0, 0, 10)
			};
		}

		private RedisClientForNHibernate BuildClientForNHibernate(string isolationKey = null)
		{
			RedisClientForNHibernate returnValue = this.BuildClientForNHibernate("www1.imusica.com.br", "nTWkcbq36Dre7qJJDqXb4G6j", 6379, isolationKey);
			return returnValue;
		}



		[TestMethod]
		public void FactoryTest()
		{
			RedisClientForNHibernate cacheClientForNHibernate = this.BuildClientForNHibernate("iMusica:NHibernateCache");
			Assert.IsNotNull(cacheClientForNHibernate);
		}

		[TestMethod]
		public void TimeOutTest()
		{
			RedisClientForNHibernate cacheClientForNHibernate = this.BuildClientForNHibernate("iMusica:NHibernateCache");
			Assert.AreEqual(cacheClientForNHibernate.CacheLifeTime, new TimeSpan(0, 0, 0, 10));
		}

		[TestMethod]
		public void PutTest()
		{
			RedisClientForNHibernate cacheClientForNHibernate = this.BuildClientForNHibernate("iMusica:NHibernateCache");
			cacheClientForNHibernate.Put("AA", "OK");
			string expected = "OK";
			string returnedValue = (string)cacheClientForNHibernate.Get("AA");
			Assert.AreEqual(expected, returnedValue);
		}

		[TestMethod]
		public void PutWithTimeOutTest()
		{
			RedisClientForNHibernate cacheClientForNHibernate = this.BuildClientForNHibernate("iMusica:NHibernateCache");
			cacheClientForNHibernate.Put("AA", "OK");
			System.Threading.Thread.Sleep(new TimeSpan(0,0,0,15));
			string returnedValue = (string)cacheClientForNHibernate.Get("AA");
			Assert.IsNull(returnedValue);
		}

		[TestMethod]
		public void RemoveTest()
		{
			RedisClientForNHibernate cacheClientForNHibernate = this.BuildClientForNHibernate("iMusica:NHibernateCache");
			cacheClientForNHibernate.Put("AA", "OK");
			cacheClientForNHibernate.Remove("AA");
			string returnedValue = (string)cacheClientForNHibernate.Get("AA");
			Assert.IsNull(returnedValue);
		}

		[TestMethod]
		public void ClearTest()
		{
			RedisClientForNHibernate cacheClientForNHibernate = this.BuildClientForNHibernate("iMusica:NHibernateCache");
			cacheClientForNHibernate.Put("A1", "OK");
			cacheClientForNHibernate.Put("A2", "OK");
			cacheClientForNHibernate.Put("A3", "OK");
			cacheClientForNHibernate.Put("A4", "OK");
			cacheClientForNHibernate.Put("A5", "OK");
			cacheClientForNHibernate.Put("A6", "OK");
			cacheClientForNHibernate.Put("A7", "OK");
			cacheClientForNHibernate.Put("A8", "OK");
			cacheClientForNHibernate.Put("A9", "OK");
			cacheClientForNHibernate.Clear();
		}

		[TestMethod]
		public void LockAndUnlockTest()
		{
			RedisClientForNHibernate cacheClientForNHibernate = this.BuildClientForNHibernate("iMusica:NHibernateCache");
			cacheClientForNHibernate.Put("A1", "OK");
			cacheClientForNHibernate.Put("A2", "OK");
			cacheClientForNHibernate.Put("A3", "OK");
			cacheClientForNHibernate.Put("A4", "OK");
			cacheClientForNHibernate.Put("A5", "OK");
			cacheClientForNHibernate.Put("A6", "OK");
			cacheClientForNHibernate.Put("A7", "OK");
			cacheClientForNHibernate.Put("A8", "OK");
			cacheClientForNHibernate.Put("A9", "OK");
			cacheClientForNHibernate.Lock("A7");


		}

	}
}
