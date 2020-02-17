using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluente.Arquitetura.Enumeradores
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFluenteUIOperation
    {
        HIDDE = 1,
        DISABLE = 2,
        FOCUS = 4,
        SHOW = 8,
        CLEAR = 16,
        SET_VALUE = 32 //Set the value of AdicionalValue
    }
}
