using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluent.Architecture.Core.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumGrupType
    {
        REGION = 0,
        TAB = 1,
        WIZARD = 2
    }
}
