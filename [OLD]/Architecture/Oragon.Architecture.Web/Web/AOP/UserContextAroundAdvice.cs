//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using AopAlliance.Intercept;
//using System.Security.Principal;
//using Oragon.Architecture.Security;
//using Spring.Threading;

//namespace Oragon.Architecture.Web.AOP
//{
//	public class UserContextAroundAdvice : IMethodInterceptor
//	{
//		public const string AuthKey = "Identity";
//		public const string ForceAuthKey = "ForcedIdentity";

//		IThreadStorage Storage { get; set; }

//		public IUserResolver UserResolver { get; set; }

//		public object Invoke(IMethodInvocation invocation)
//		{
//			object returnValue = null;
//			IIdentity identity = this.GetIdentity();
//			object userProfile = null;
//			if (identity.IsAuthenticated)
//				userProfile = this.UserResolver.Resolve(identity);
//			this.Storage.SetData(UserContextAroundAdvice.AuthKey, userProfile);
//			try
//			{
//				returnValue = invocation.Proceed();
//			}
//			finally
//			{
//				//Spring.Threading.LogicalThreadContext.SetData(UserContextAroundAdvice.AuthKey, null);
//				//Spring.Threading.LogicalThreadContext.FreeNamedDataSlot(UserContextAroundAdvice.AuthKey);
//			}
//			return returnValue;
//		}

//		private IIdentity GetIdentity()
//		{
//			object bypassIdentity = this.Storage.GetData(UserContextAroundAdvice.ForceAuthKey);
//			IIdentity returnValue = null;
//			if ((bypassIdentity != null) && (bypassIdentity is IIdentity))
//				returnValue = (IIdentity)bypassIdentity;
//			else
//			{
//				if(System.Web.HttpContext.Current != null)
//					returnValue = System.Web.HttpContext.Current.User.Identity;
//				else
//					returnValue = new System.Security.Principal.GenericIdentity(@"afonseca", "FORMS");
//			}
				

//			return returnValue;
//		}
//	}
//}
