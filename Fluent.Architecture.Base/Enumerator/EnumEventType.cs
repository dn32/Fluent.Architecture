using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluent.Architecture.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumEventType
    {
        CHANGED,
        ADD,
        DELETE
    }
}
