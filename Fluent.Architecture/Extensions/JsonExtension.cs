
using Fluent.Architecture.Extensions;
using Newtonsoft.Json;

namespace Fluent.Architecture.Core.Extensions
{
    public static class JsonExtension
    {
        public static T JsonObjectToObject<T>(this object jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonObject));
        }

        public static string ToFluentJsonOrPrimitive(this object obj, Formatting formatting = Formatting.None)
        {
            if(obj == null) { return null; }
            var type = obj.GetType();
            if (type.IsPrimitiveOrPrimitiveNulable()) { return obj.ToString(); }
            return obj.ToFluentJson(formatting);
        }


        public static T ToFluentObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonExtensionBase.JsonSerializerSettings);
        }

        public static string ToFluentJsonStringNormalized(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return text; }

            if (JsonExtensionBase.JsonSerializerSettings.ContractResolver.GetType().Name == "CamelCasePropertyNamesContractResolver")
            {
                return text.ToCamelCase();
            }

            return text;
        }
    }
}
