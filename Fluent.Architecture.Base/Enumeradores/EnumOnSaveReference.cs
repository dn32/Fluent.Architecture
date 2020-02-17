using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluente.Arquitetura.Nucleo.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumOnSaveReference
    {
        ADD_AND_UPDATE = 0,
        ADD = 1,
        IGNORE = 2
    }
}
