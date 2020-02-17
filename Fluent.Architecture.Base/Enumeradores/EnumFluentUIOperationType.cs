using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluente.Arquitetura.Enumeradores
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFluenteUIOperationType
    {
        EQUAL = 1,
        NOT_EQUAL = 2,
    }
}
