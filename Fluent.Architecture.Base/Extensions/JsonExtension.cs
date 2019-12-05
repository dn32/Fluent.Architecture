using Newtonsoft.Json;

namespace Fluent.Architecture.Core.Extensions
{
    public static class JsonExtensionBase
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; }

        public static string ToFluentJson(this object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting, JsonSerializerSettings);
        }
    }
}
