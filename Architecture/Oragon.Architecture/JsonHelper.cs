using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Oragon.Architecture
{

    /// <summary>
    /// Implementa um serializador Json
    /// </summary>
    public static class JsonHelper
    {
        public enum ConverterType
        {
            Serialization,
            Deserialization
        }

        private static JsonConverter[] GetConverters(ConverterType type)
        {
            if (type == ConverterType.Serialization)
                return new List<JsonConverter>() { new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter() }.ToArray();

            if (type == ConverterType.Deserialization)
                return new List<JsonConverter>() { new Newtonsoft.Json.Converters.IsoDateTimeConverter() }.ToArray();

            throw new System.NotSupportedException("Tipo de converter inválido");
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


        public static T ConvertUsingSerialization<T>(object objectToSerialize)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters = JsonHelper.GetConverters(ConverterType.Serialization);
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            string data = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented, settings);
            T returnValue = JsonConvert.DeserializeObject<T>(data, settings);
            return returnValue;
        }


    }
}
