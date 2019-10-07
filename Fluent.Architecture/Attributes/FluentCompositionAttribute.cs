using Fluent.Architecture.Core.Models;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentCompositionAttribute : FluentReferenceAttribute
    {
        public FluentJsonSchema Form { get; set; }
    }
}
