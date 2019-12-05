using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluent.Architecture.Core.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFluentDisplay
    {
        Show,
        Hidden,
    }
}
