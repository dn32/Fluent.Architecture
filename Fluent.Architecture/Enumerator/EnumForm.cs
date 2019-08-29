using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluent.Architecture.Core.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumForm
    {
        //[EnumMember(Value = "None")]
        NONE = 0,

        // [EnumMember(Value = "Textbox")]
        TEXTBOX = 1,

        //[EnumMember(Value = "Combobox")]
        COMBOBOX = 2,

        //[EnumMember(Value = "Number")]
        NUMBER = 4,

        // [EnumMember(Value = "Date picker")]
        DATEPICKER = 5,

        //[EnumMember(Value = "File")]
        FILE = 6,

        //[EnumMember(Value = "Modal")]
        MODAL = 6,

        //[EnumMember(Value = "Form")]
        FORM = 7,

        HIDDEN = 8
    }
}
