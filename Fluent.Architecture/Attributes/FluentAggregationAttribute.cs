using Fluent.Architecture.Core.Enumerator;
using Newtonsoft.Json;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentAggregationAttribute : FluentReferenceAttribute
    {
        public string Display { get; set; }
        public string[] PropertiesForFindByProximity { get; set; }

        [JsonIgnore]
        public string PropertyForFindByProximity
        {
            get
            {
                return PropertiesForFindByProximity?.Length > 0 ? PropertiesForFindByProximity[0] : null;
            }
            set
            {
                PropertiesForFindByProximity = new[] { value };
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
