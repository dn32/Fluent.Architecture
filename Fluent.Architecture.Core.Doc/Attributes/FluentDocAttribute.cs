using Fluent.Architecture.Core.Enumerator;
using System;

namespace Fluent.Architecture.Core.Doc.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class FluentDocAttribute : Attribute
    {
        public EnumFluentDisplay Display { get; set; }

        public FluentDocAttribute()
        {
        }

        public FluentDocAttribute(EnumFluentDisplay display)
        {
            Display = display;
        }
    }
}
