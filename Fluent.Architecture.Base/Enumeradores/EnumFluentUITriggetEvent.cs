using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluente.Arquitetura.Enumeradores
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFluenteUITriggetEvent
    {
        NONE = 0,
        CHANGE = 1
    }
}
