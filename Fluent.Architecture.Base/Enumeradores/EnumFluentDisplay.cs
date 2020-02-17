using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluente.Arquitetura.Nucleo.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFluenteDisplay
    {
        Show,
        Hidden,
    }
}
