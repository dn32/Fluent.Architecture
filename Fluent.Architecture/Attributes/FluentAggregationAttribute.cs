using Fluent.Architecture.Core.Enumerator;
using Newtonsoft.Json;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentAggregationAttribute : FluentReferenceAttribute
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

        public FluentFilterAttribute FluentFilter { get; set; }
    }


    public class FluentLoggingAttribute : Attribute
    {
        public EnumFluentDisplay Display { get; set; }

        public FluentLoggingAttribute(EnumFluentDisplay display = EnumFluentDisplay.Show)
        {
            Display = display;
        }
    }
}
