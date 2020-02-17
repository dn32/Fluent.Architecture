using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluente.Arquitetura.Nucleo.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumForm
    {
        NONE = 0,
        TEXTBOX = 1,
        COMBOBOX = 2,
        NUMBER = 4,
        DATEPICKER = 5,
        FILE = 6,
        MODAL = 6,
        FORM = 7,
        HIDDEN = 8
    }
}
