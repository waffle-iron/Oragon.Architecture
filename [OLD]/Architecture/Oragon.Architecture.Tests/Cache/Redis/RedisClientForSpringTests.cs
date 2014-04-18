using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Mapping;
using Oragon.Architecture.Cache.Redis;
using Oragon.Architecture.Extensions;
using ServiceStack.Redis;

namespace Oragon.Architecture.Tests.Cache.Redis
{
	[TestClass]
	public class RedisClientForSpringTests
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

		private RedisClientForSpring BuildClientForSpring(string serverName, string auth, int? port, string isolationKey)
		{
			IRedisClient redisClient = this.BuildRedisClient(serverName: serverName, auth: auth, port: port);
			return new RedisClientForSpring(redisClient, isolationKey);
		}

		private RedisClientForSpring BuildClientForSpring(string isolationKey = null)
		{
			RedisClientForSpring returnValue = this.BuildClientForSpring("www1.imusica.com.br", "nTWkcbq36Dre7qJJDqXb4G6j", 6379, isolationKey);
			return returnValue;
		}


		[TestMethod]
		public void CreateRedisClientTest()
		{
			IRedisClient client = this.BuildRedisClient("www1.imusica.com.br", "nTWkcbq36Dre7qJJDqXb4G6j", 6379);
			Assert.IsNotNull(client);
		}

		[TestMethod]
		public void BuildClientForSpringTest()
		{
			RedisClientForSpring client = this.BuildClientForSpring("iMusica:Tests");
			Assert.IsNotNull(client);
		}


		[TestMethod]
		public void RedisInsertTest()
		{
			RedisClientForSpring client = this.BuildClientForSpring("iMusica:Tests");
			client.Insert("RedisInsertTest", "OK");
			string expected = "OK";
			string returnedValue = (string)client.Get("RedisInsertTest");
			Assert.AreEqual(expected, returnedValue);
			client.Clear();
		}

		public class People
		{
			public List<Pet> Pets { get; set; }

			public string Name { get; set; }

			public int Age { get; set; }

			public People()
			{
				this.Pets = new List<Pet>();
			}

			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("My name is {0}, i have {1} years old and have {2} pets.", this.Name, this.Age,
					this.Pets.Any() ? this.Pets.Count.ToString() : "no");
				stringBuilder.AppendLine();

				foreach (Pet pet in this.Pets)
				{
					stringBuilder.AppendLine(pet.ToString());
				}
				return stringBuilder.ToString();
			}
		}

		public class Pet
		{
			public string Name { get; set; }
		}

		public class Dog : Pet
		{
			public override string ToString()
			{
				return string.Format("I'm a Dog, my name is {0}! Au au");
			}
		}
		public class Cat : Pet
		{
			public override string ToString()
			{
				return string.Format("I'm a Cat, my name is {0}! Miau");
			}
		}

		[TestMethod]
		public void RedisInsertComplexObjectTest()
		{
			RedisClientForSpring client = this.BuildClientForSpring("iMusica:Tests:People");
			var luiz = new People
			{
				Name = "Luiz Carlos",
				Age = 30,
			};
			luiz.Pets.Add(new Dog() { Name = "Ziggy" });
			luiz.Pets.Add(new Cat() { Name = "Moly" });
			client.Insert("Luiz", luiz);
			luiz = (People)client.Get("Luiz");

			Assert.AreEqual(luiz.Name, "Luiz Carlos");
			Assert.AreEqual(luiz.Age, 30);
			Assert.AreEqual(luiz.Pets.Count, 2);
			Assert.AreEqual(luiz.Pets.First().Name, "Ziggy");
			Assert.AreEqual(luiz.Pets.First().GetType(), typeof(Dog));

			Assert.AreEqual(luiz.Pets.Last().Name, "Moly");
			Assert.AreEqual(luiz.Pets.Last().GetType(), typeof(Cat));

		}


		[TestMethod]
		public void RedisInsertTestWithTimeout()
		{
			TimeSpan timeout = new TimeSpan(0, 0, 0, 15);
			RedisClientForSpring client = this.BuildClientForSpring("iMusica:Tests:RedisInsertTestWithTimeout");
			client.Insert("RedisInsertTestWithTimeout", "OK", timeout);
			string expected = "OK";
	
			string returnedValue = (string)client.Get("RedisInsertTestWithTimeout");
			Assert.AreEqual(expected, returnedValue);

			Thread.Sleep(new TimeSpan(0, 0, 0, 5));
			returnedValue = (string)client.Get("RedisInsertTestWithTimeout");
			Assert.AreEqual(expected, returnedValue);

			Thread.Sleep(new TimeSpan(0, 0, 0, 15));
			returnedValue = (string)client.Get("RedisInsertTestWithTimeout");
			Assert.IsNull(returnedValue);

			client.Clear();
		}

		[TestMethod]
		public void RedisCount()
		{
			RedisClientForSpring client = this.BuildClientForSpring("iMusica:Tests:New");
			client.Insert("RedisCount1", "OK");
			client.Insert("RedisCount2", "OK");
			client.Insert("RedisCount3", "OK");
			int expected = 3;
			int returnedValue = client.Count;
			Assert.AreEqual(expected, returnedValue);

			client.Remove("RedisCount1");
			client.Remove("RedisCount2");
			client.Remove("RedisCount3");
			expected = 0;
			returnedValue = client.Count;
			Assert.AreEqual(expected, returnedValue);
			client.Clear();
		}

		[TestMethod]
		public void RedisClear1()
		{
			RedisClientForSpring client = this.BuildClientForSpring("iMusica:Tests:ClearTest");
			client.Insert("RedisClear1", "OK");
			client.Insert("RedisClear2", "OK");
			client.Insert("RedisClear3", "OK");
			int expected = 3;
			int returnedValue = client.Count;
			Assert.AreEqual(expected, returnedValue);

			client.Clear();
			expected = 0;
			returnedValue = client.Count;
			Assert.AreEqual(expected, returnedValue);
			client.Clear();
		}



	}
}
