using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oragon.Architecture.Tests.Cache.Redis.AOP
{
	[TestClass]
	public class ExampleTest : TestBase
	{
		public IExampleService ExampleService { get; set; }

		[TestMethod]
		public void IOCTest()
		{
			Assert.IsNotNull(this.ExampleService);
		}


		[TestMethod]
		public void StoreTest()
		{
			ExampleModel expectation = new ExampleModel() { Nome1 = "A", Nome2 = "B" };
			this.ExampleService.Store("AA", expectation);
			ExampleModel returnedValue = this.ExampleService.Retrieve("AA");
			Assert.IsNotNull(returnedValue);
			Assert.AreEqual(expectation.Nome1, returnedValue.Nome1);
			Assert.AreEqual(expectation.Nome2, returnedValue.Nome2);
		}


	}
}
