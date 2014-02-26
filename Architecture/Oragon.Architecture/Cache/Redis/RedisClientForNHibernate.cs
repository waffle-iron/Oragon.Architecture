using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cache;
using Oragon.Architecture.Extensions;
using ServiceStack.Redis;

namespace Oragon.Architecture.Cache.Redis
{
	public class RedisClientForNHibernate : RedisProviderBase, NHibernate.Cache.ICache
	{
		private class LockItem
		{
			public string Key { get; set; }
			public IDisposable Locker { get; set; }
		}

		private List<LockItem> LockedItens;

		public RedisClientForNHibernate(IRedisClient redisClient, string isolationKey)
			: base(redisClient, isolationKey)
		{
			this.LockedItens = new List<LockItem>();
			if (this.IsolationKey.IsNullOrWhiteSpace())
				this.IsolationKey = "NHibernate-Cache";
		}

		public void Clear()
		{
			string formattedKey = base.GetKey();
			List<string> keys = this.NativeClient.SearchKeys(formattedKey + "*");
			this.NativeClient.RemoveAll(keys.Cast<string>());
		}

		public void Destroy()
		{
			this.Clear();
		}

		public void Lock(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			IDisposable locker = this.NativeClient.AcquireLock(formattedKey);
			this.LockedItens.AddIf(it => this.LockedItens.Any(it2 => it.Key == it2.Key), new LockItem { Key = formattedKey, Locker = locker });
		}

		public void Unlock(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			LockItem lockItem = this.LockedItens.FirstOrDefault(it => it.Key == formattedKey);
			if (lockItem.IsNotNull())
			{
				lockItem.Locker.Dispose();
				this.LockedItens.Remove(lockItem);
			}
		}

		public long NextTimestamp()
		{
			return Timestamper.Next();
		}

		public int Timeout
		{
			get
			{
				return 245760000;
			}
		}

		public string RegionName { get;  set; }

		public TimeSpan CacheLifeTime { get;  set; }

		public object Get(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			return this.NativeClient.Get<Object>(formattedKey);
		}

		public void Put(object key, object value)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			this.NativeClient.Set(formattedKey, value, this.CacheLifeTime);
		}
		
		public void Remove(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			this.NativeClient.Remove(formattedKey);
		}

	}
}
