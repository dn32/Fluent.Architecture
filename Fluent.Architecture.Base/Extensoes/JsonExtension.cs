using Newtonsoft.Json;

namespace Fluente.Arquitetura.Nucleo.Extensoes
{
    //Todo - 001 Testar
    public static class JsonExtensionBase
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; }

        public static string ToFluenteJson(this object obj, Formatting formatting = Formatting.None) =>
            JsonConvert.SerializeObject(obj, formatting, JsonSerializerSettings);
    }
}
