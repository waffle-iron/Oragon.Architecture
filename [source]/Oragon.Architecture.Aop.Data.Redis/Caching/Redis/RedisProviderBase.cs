﻿using Newtonsoft.Json;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.Serialization;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Architecture.Caching.Redis
{
	[CLSCompliant(false)]
	public class RedisProviderBase : IDisposable
	{
		#region Public Constructors

		public RedisProviderBase(IRedisClient redisClient, string isolationKey)
		{
			this.NativeClient = redisClient;
			this.IsolationKey = isolationKey;
		}

		#endregion Public Constructors

		#region Public Properties

		public IRedisClient NativeClient { get; private set; }

		#endregion Public Properties

		#region Protected Properties

		protected string IsolationKey { get; set; }

		#endregion Protected Properties

		#region Serialization

		private JsonSerializerSettings _serializerSettings;

		protected JsonSerializerSettings SerializerSettings
		{
			get
			{
				if (this._serializerSettings.IsNull())
					this._serializerSettings = this.BuildSerializer();
				return this._serializerSettings;
			}
		}

		protected virtual JsonSerializerSettings BuildSerializer()
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				MaxDepth = null,
				Formatting = Newtonsoft.Json.Formatting.None,
				PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All,
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
				MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
				ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Auto,
				TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects,
				Converters = JsonHelper.GetConverters(JsonHelper.ConverterType.Deserialization)
			};
			return settings;
		}

		#endregion Serialization

		#region "Key Management"

		public string GetKey(string key = null)
		{
			string[] keys = key.IsNullOrWhiteSpace() ? new string[] { } : new string[] { key };
			return this.GetKey(keys);
		}

		public string GetKey(params string[] keys)
		{
			List<string> keyList = keys.IsNotNull() ? new List<string>(keys) : new List<string>();
			if (this.IsolationKey.IsNotNullOrWhiteSpace())
				keyList.Insert(0, this.IsolationKey);
			keyList = this.ReorganizeKeys(keyList);
			string key = this.BuildKey(keyList);
			return key;
		}

		private string BuildKey(List<string> keys)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < keys.Count; i++)
			{
				string key = keys[i];
				builder.Append(key);
				if (i < keys.Count - 1)
					builder.Append(":");
			}
			return builder.ToString();
		}

		private List<string> ReorganizeKeys(List<string> keys)
		{
			List<string> allKeysList = new List<string>();
			for (int i = 0; i < keys.Count; i++)
			{
				string key = keys[i];
				string[] tmpKeys = key.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
				allKeysList.AddRange(tmpKeys);
			}
			return allKeysList;
		}

		#endregion "Key Management"

		#region Public Methods

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion Public Methods

		#region Protected Methods

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
				if (this.NativeClient != null)
				{
					this.NativeClient.Dispose();
					this.NativeClient = null;
				}
			}
		}

		#endregion Protected Methods
	}
}