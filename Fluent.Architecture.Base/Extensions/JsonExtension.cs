using Newtonsoft.Json;

namespace Fluent.Architecture.Core.Extensions
{
    //Todo - 001 Testar
    public static class JsonExtensionBase
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; }

        public static string ToFluentJson(this object obj, Formatting formatting = Formatting.None) =>
            JsonConvert.SerializeObject(obj, formatting, JsonSerializerSettings);
    }
}
