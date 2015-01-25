using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Oragon.Architecture.Serialization
{
	/// <summary>
	///     Implementa um serializador Json
	/// </summary>
	public static class JsonHelper
	{
		#region Public Enums

		public enum ConverterType
		{
			Serialization,
			Deserialization
		}

		#endregion Public Enums

		#region Public Methods

		public static T ConvertUsingSerialization<T>(object objectToSerialize)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Converters = JsonHelper.GetConverters(ConverterType.Serialization);
			settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

			string data = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented, settings);
			T returnValue = JsonConvert.DeserializeObject<T>(data, settings);
			return returnValue;
		}

		public static T Deserialize<T>(string data)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Converters = JsonHelper.GetConverters(ConverterType.Deserialization);

			T returnValue = JsonConvert.DeserializeObject<T>(data, settings);
			return returnValue;
		}

		public static object Deserialize(string data, Type type)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Converters = JsonHelper.GetConverters(ConverterType.Deserialization);

			object returnValue = JsonConvert.DeserializeObject(data, type, settings);
			return returnValue;
		}

		public static JsonConverter[] GetConverters(ConverterType type)
		{
			if (type == ConverterType.Serialization)
				return new List<JsonConverter>() { new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter() }.ToArray();

			if (type == ConverterType.Deserialization)
				return new List<JsonConverter>() { new Newtonsoft.Json.Converters.IsoDateTimeConverter() }.ToArray();

			return null;
		}

		public static string Serialize(object objectToSerialize)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Converters = JsonHelper.GetConverters(ConverterType.Serialization);
			//settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
			//settings.PreserveReferencesHandling = PreserveReferencesHandling.All;

			string returnValue = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented, settings);
			return returnValue;
		}

		#endregion Public Methods
	}
}