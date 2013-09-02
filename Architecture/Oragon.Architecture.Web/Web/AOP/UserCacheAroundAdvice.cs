//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Oragon.Architecture.AOP;
//using System.Security.Principal;

//namespace Oragon.Architecture.Web.AOP
//{
//	public class UserCacheAroundAdvice : CacheAroundAdvice
//	{
//		string Environment { get; set; }

//		protected override string GetCacheKey(AopAlliance.Intercept.IMethodInvocation invocation, bool includeParameterValues)
//		{
//			IIdentity identity = Spring.Threading.LogicalThreadContext.GetData(UserContextAroundAdvice.AuthKey) as IIdentity;
//			string originalCacheKey = base.GetCacheKey(invocation, includeParameterValues);
//			string additionalKeyInfo = string.Format("[name:{0}|environment:{1}]", identity.Name, this.Environment);
//			string cacheKey = string.Concat(originalCacheKey, additionalKeyInfo);
//			return cacheKey;
//		}

//	}
//}
