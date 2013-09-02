using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using Spring.Threading;
using Oragon.Architecture.Security.Authorization.Profile;

namespace Oragon.Architecture.Security.Authorization.AOP
{
	public class AuthorizationProfileAroundAdvice : IMethodInterceptor
	{
		const string AuthorizationKey = "AuthorizationProfile";

		IThreadStorage CallContextStorage { get; set; }

		IThreadStorage CacheStorage { get; set; }

		private void Set(IThreadStorage storage, string key, AuthorizationProfile authorizationProfile)
		{
			storage.SetData(key, authorizationProfile);
		}

		private AuthorizationProfile Get(IThreadStorage storage, string key)
		{
			AuthorizationProfile returnValue = storage.GetData(key) as AuthorizationProfile;
			return returnValue;
		}

		public object Invoke(IMethodInvocation invocation)
		{
			AuthorizationProfile authorizationProfile = this.Get(this.CacheStorage, AuthorizationKey);
			if (authorizationProfile == null)
				authorizationProfile = new AuthorizationProfile();

			int oldVersion = authorizationProfile.Version;
			this.Set(this.CallContextStorage, AuthorizationKey, authorizationProfile);
			
			object returnValue = invocation.Proceed();

			authorizationProfile = this.Get(this.CallContextStorage, AuthorizationKey);
			int newVersion = authorizationProfile.Version;

			if (oldVersion != newVersion)
				this.Set(this.CacheStorage, AuthorizationKey, authorizationProfile);
			return returnValue;
		}
	}
}
