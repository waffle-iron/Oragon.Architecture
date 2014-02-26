using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Cache.Redis.AOP
{
	public interface IExampleService
	{
		void Store(string key, ExampleModel model);

		ExampleModel Retrieve(string key);
	}
}
