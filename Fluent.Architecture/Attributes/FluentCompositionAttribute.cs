using Fluent.Architecture.Core.Entities;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentCompositionAttribute : FluentReferenceAttribute
    {
        public FluentJsonSchema Form { get; set; }
    }
}
