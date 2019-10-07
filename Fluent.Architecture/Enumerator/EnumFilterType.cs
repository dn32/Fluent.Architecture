using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fluent.Architecture.Core.Enumerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumFilterType
    {
        //Numérico
        GREATER, // Maior que
        SMALLER, // Menor que

        //String
        START_WITH, // Inicia com
        ENDS_WITH, // Termina com
        CONTAINS, // Contém

        //All
        EQUAL, // igual a
        NULL, // é nulo

        //Bool
        TRUE, // é true
        FALSE, // é false
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumJunctionType
    {
        AND,
        OR
    }

    public class Filter
    {
        public EnumFilterType FilterType { get; set; }
        public EnumJunctionType JunctionType { get; set; }
        public bool IsReverse { get; set; }
        public string Value { get; set; }
        public string PropertyName { get; set; }
        public bool Including { get; set; }
    }
}
