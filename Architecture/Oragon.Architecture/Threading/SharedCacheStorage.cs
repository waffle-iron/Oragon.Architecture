using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Threading;
using CacheServer = SharedCache.WinServiceCommon.Provider.Cache.IndexusDistributionCache;


namespace Oragon.Architecture.Threading
{
	public class SharedCacheStorage : IThreadStorage
	{
		IKeyParser KeyParser { get; set; }
		TimeSpan CacheTimeOut { get; set; }

		private string ResolveName(string name)
		{
			string newName = this.KeyParser.GetName(name);
			return newName;
		}

		public void FreeNamedDataSlot(string name)
		{
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				RetryManager.Try(delegate()
				{
					CacheServer.SharedCache.Remove(newName);
				}, 2, false);
			}
		}

		public object GetData(string name)
		{
			object returnValue = null;
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				RetryManager.Try(delegate()
				{
					returnValue = CacheServer.SharedCache.Get(newName);
				}, 2, false);
			}
			return returnValue;
		}

		public void SetData(string name, object value)
		{
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				RetryManager.Try(delegate(){
					CacheServer.SharedCache.Add(newName, value, DateTime.Now.Add(this.CacheTimeOut));
				} , 2, false );
			}
		}
	}
}
