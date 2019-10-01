
using Fluent.Architecture.Extensions;
using Newtonsoft.Json;

namespace Fluent.Architecture.Core.Extensions
{
    public static class JsonExtension
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; }

        public static T JsonObjectToObject<T>(this object jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonObject));
        }

        public static string ToFluentJson(this object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting, JsonSerializerSettings);
        }

        public static string ToFluentJsonStringNormalized(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return text; }

            if(JsonSerializerSettings.ContractResolver.GetType().Name == "CamelCasePropertyNamesContractResolver")
            {
               return text.ToCamelCase();
            }

            return text;
        }
    }
}
