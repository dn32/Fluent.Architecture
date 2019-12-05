
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluent.Architecture.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFluentUIOperationType
    {
        EQUAL = 1,
        NOT_EQUAL = 2,
    }
}
