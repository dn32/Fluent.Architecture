
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluent.Architecture.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFluentUITriggetEvent
    {
        NONE = 0,
        CHANGE = 1
    }
}
