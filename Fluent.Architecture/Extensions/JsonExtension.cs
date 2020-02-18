using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Extensoes;
using Newtonsoft.Json;

namespace Fluente.Arquitetura.Nucleo.Extensoes
{
    public static class JsonExtension
    {
        public static T JsonObjectToObject<T>(this object jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonObject));
        }

        public static string ToFluenteJsonOrPrimitive(this object obj, Formatting formatting = Formatting.None)
        {
            if (obj == null) { return null; }
            var type = obj.GetType();
            if (type.IsPrimitiveOrPrimitiveNulable()) { return obj.ToString(); }
            return obj.SerializarParaFluenteJson(formatting);
        }


        public static T ToFluenteObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, ExtensoesJson.JsonSerializerSettings);
        }

        public static string ToFluenteJsonStringNormalized(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return text; }

            if (ExtensoesJson.JsonSerializerSettings?.ContractResolver?.GetType()?.Name == "CamelCasePropertyNamesContractResolver")
            {
                return text.ToCamelCase();
            }

            return text;
        }
    }
}
