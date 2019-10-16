using Fluent.Architecture.Core.Enumerator;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FluentAPIControllerAttribute : Attribute
    {
        public bool AutomaticGeneration { get; set; }

        public FluentAPIControllerAttribute()
        {
            AutomaticGeneration = true;
        }

        public FluentAPIControllerAttribute(bool automaticGeneration = true)
        {
            AutomaticGeneration = automaticGeneration;
        }
    }
}
