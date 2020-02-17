using Fluente.Arquitetura.Nucleo.Enumerator;
using Newtonsoft.Json;
using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteAggregationAttribute : FluenteReferenceAttribute
    {
        public string Display { get; set; }
        public string[] PropertiesForFind { get; set; }
        public bool AllowAdd { get; set; }

        [JsonIgnore]
        public string PropertyForFind
        {
            get
            {
                return PropertiesForFind?.Length > 0 ? PropertiesForFind[0] : null;
            }
            set
            {
                PropertiesForFind = new[] { value };
            }
        }

        public FluenteFilterAttribute FluenteFilter { get; set; }
    }


    public class FluenteLoggingAttribute : Attribute
    {
        public EnumFluenteDisplay Display { get; set; }

        public FluenteLoggingAttribute(EnumFluenteDisplay display = EnumFluenteDisplay.Show)
        {
            Display = display;
        }
    }
}
