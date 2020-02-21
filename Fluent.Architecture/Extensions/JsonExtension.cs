using dn32.infra.extensoes;
using Newtonsoft.Json;

namespace dn32.infra.Nucleo.Extensoes
{
    public static class JsonExtension
    {
        public static T JsonObjectToObject<T>(this object jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonObject));
        }

        public static string ToDnJsonOrPrimitive(this object obj, Formatting formatting = Formatting.None)
        {
            if (obj == null) { return null; }
            var type = obj.GetType();
            if (type.IsPrimitiveOrPrimitiveNulable()) { return obj.ToString(); }
            return obj.SerializarParaDnJson(formatting);
        }


        public static T ToDnObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, ExtensoesJson.ConfiguracoesDeSerializacao);
        }

        public static string ToDnJsonStringNormalized(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return text; }

            if (ExtensoesJson.ConfiguracoesDeSerializacao?.ContractResolver?.GetType()?.Name == "CamelCasePropertyNamesContractResolver")
            {
                return text.ToCamelCase();
            }

            return text;
        }
    }
}
