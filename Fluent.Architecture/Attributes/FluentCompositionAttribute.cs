using System;
using Fluent.Architecture.Core.Entities;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentCompositionAttribute : FluentReferenceAttribute
    {
        public FluentJsonSchema Form { get; set; }
    }
}
