using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Oragon.Architecture.Extensions;
using ServiceStack;
using ServiceStack.Redis;

namespace Oragon.Architecture.Cache.Redis
{
	public class RedisClientForSpring : RedisProviderBase, Spring.Caching.ICache
	{

		public RedisClientForSpring(IRedisClient redisClient, string isolationKey)
			: base(redisClient, isolationKey)
		{
		}

		public void Clear()
		{
			string formattedKey = base.GetKey();
			List<string> keys = this.RedisClient.SearchKeys(formattedKey + "*");
			this.RemoveAll(keys);
		}

		public int Count
		{
			get
			{
				string formattedKey = base.GetKey();
				return this.RedisClient.SearchKeys(formattedKey + "*").Count;
			}
		}

		public object Get(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			string jsonSerialized = this.RedisClient.Get<string>(formattedKey);
			object deserializeObject = null;
			if (jsonSerialized.IsNotNullOrWhiteSpace())
				deserializeObject = JsonConvert.DeserializeObject(jsonSerialized, this.SerializerSettings);
			return deserializeObject;
		}

		public void Insert(object key, object value, TimeSpan timeToLive)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			string jsonSerialized = JsonConvert.SerializeObject(value, this.SerializerSettings);
			this.RedisClient.Set(formattedKey, jsonSerialized, timeToLive);
		}

		public void Insert(object key, object value)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			string jsonSerialized = JsonConvert.SerializeObject(value, this.SerializerSettings);
			this.RedisClient.Set(formattedKey, jsonSerialized);
		}

		public System.Collections.ICollection Keys
		{
			get
			{
				string formattedKey = base.GetKey();
				List<string> keys = this.RedisClient.SearchKeys(formattedKey + "*");
				return keys;
			}
		}

		public void Remove(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			this.RedisClient.Remove(formattedKey);
		}

		public void RemoveAll(System.Collections.ICollection keys)
		{
			this.RedisClient.RemoveAll(keys.Cast<string>());
		}
	}
}
