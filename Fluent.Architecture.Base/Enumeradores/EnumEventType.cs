using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluente.Arquitetura.Enumeradores
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumEventType
    {
        CHANGED,
        ADD,
        DELETE
    }
}
