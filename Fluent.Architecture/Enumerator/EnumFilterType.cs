using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace Fluent.Architecture.Core.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFilterType
    {
        //Numérico
        [Description("Value must be greater than (...)")]
        GREATER, // Maior que

        [Description("The value must be less than (...)")]
        SMALLER, // Menor que

        //String

        [Description("The text must start with (...)")]
        START_WITH, // Inicia com

        [Description("The text must end with (...)")]
        ENDS_WITH, // Termina com

        [Description("The text must contain (...)")]
        CONTAINS, // Contém

        //All
        [Description("The value must be equal to (...)")]
        EQUAL, // igual a

        [Description("Value must be null")]
        NULL, // é nulo

        //Bool
        [Description("Value must be true")]
        TRUE, // é true

        [Description("Value must be false")]
        FALSE, // é false
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumJunctionType
    {
        [Description("\"E\" Junction")] //
        AND,

        [Description("\"OR\" Junction")] //
        OR
    }

    public class Filter
    {
        [Description("The type of filter to use in the query")]
        public EnumFilterType FilterType { get; set; }

        [Description("The type of junction with the next filter, if any")] 
        public EnumJunctionType JunctionType { get; set; }

        [Description("If you want to reverse the operation. Example: (\"FilterType = EQUAL\" and \"Reverse = true\" equals \"different\")")] 
        public bool IsReverse { get; set; }

        [Description("Value to use in comparison")] 
        public string Value { get; set; }

        [Description("The property whose value will be used for comparison with the \"Value\"")]
        public string PropertyName { get; set; }

        [Description("If it is inclusive. For example: (\"FilterType = GREATER\", \"Value = 10\" and \"Including = true\" equals \">= 10\")")] 
        public bool Including { get; set; }
    }
}
