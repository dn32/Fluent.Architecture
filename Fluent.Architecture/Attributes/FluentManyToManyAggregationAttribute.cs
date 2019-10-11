using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentManyToManyAggregationAttribute : FluentAggregationAttribute
    {
        public bool IsManyToMany { get; } = true;
    }
}
