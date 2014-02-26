using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NH = NHibernate;
using NHibernate.Linq;
using Spring.Objects.Factory.Attributes;
using Oragon.Architecture.Business;

namespace Oragon.Architecture.AOP.Data.Redis
{

	public class RedisDataProcess : Oragon.Architecture.AOP.Data.Abstractions.AbstractDataProcess<RedisContext, RedisContextAttribute>
	{

	}


}
