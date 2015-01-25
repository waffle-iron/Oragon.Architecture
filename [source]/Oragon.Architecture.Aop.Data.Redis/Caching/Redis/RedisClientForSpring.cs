using Newtonsoft.Json;
using Oragon.Architecture.Extensions;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Architecture.Caching.Redis
{
	[CLSCompliant(false)]
	public class RedisClientForSpring : RedisProviderBase, Spring.Caching.ICache
	{
		#region Public Constructors

		public RedisClientForSpring(IRedisClient redisClient, string isolationKey)
			: base(redisClient, isolationKey)
		{
		}

		#endregion Public Constructors

		#region Public Properties

		public int Count
		{
			get
			{
				string formattedKey = base.GetKey();
				return this.NativeClient.SearchKeys(formattedKey + "*").Count;
			}
		}

		public System.Collections.ICollection Keys
		{
			get
			{
				string formattedKey = base.GetKey();
				List<string> keys = this.NativeClient.SearchKeys(formattedKey + "*");
				return keys;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public void Clear()
		{
			string formattedKey = base.GetKey();
			List<string> keys = this.NativeClient.SearchKeys(formattedKey + "*");
			this.RemoveAll(keys);
		}

		public object Get(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			string jsonSerialized = this.NativeClient.Get<string>(formattedKey);
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
			this.NativeClient.Set(formattedKey, jsonSerialized, timeToLive);
		}

		public void Insert(object key, object value)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			string jsonSerialized = JsonConvert.SerializeObject(value, this.SerializerSettings);
			this.NativeClient.Set(formattedKey, jsonSerialized);
		}

		public void Remove(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			this.NativeClient.Remove(formattedKey);
		}

		public void RemoveAll(System.Collections.ICollection keys)
		{
			this.NativeClient.RemoveAll(keys.Cast<string>());
		}

		#endregion Public Methods
	}
}