using Fluent.Architecture.Core.Enumerator;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentAggregationAttribute : FluentReferenceAttribute
    {
        public string Display { get; set; }
        public string PropertyForFindByProximity { get; set; }
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
