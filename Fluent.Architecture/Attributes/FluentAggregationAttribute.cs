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

    [AttributeUsage(AttributeTargets.Property)]
    public class FluentManyToManyAggregationAttribute : FluentAggregationAttribute
    {
        public bool IsManyToMany { get; } = true;
        public string Plural { get; set; }
        public string Junction { get; set; }
        public string Singular { get; set; }
        public bool IsMasculine { get; set; }
    }
}
