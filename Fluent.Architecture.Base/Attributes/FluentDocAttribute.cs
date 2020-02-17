using Fluent.Architecture.Core.Enumerator;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property)]
    public class FluentDocAttribute : Attribute
    {
        public EnumFluentDisplay Display { get; private set; }
        
        public FluentDocAttribute(EnumFluentDisplay display = EnumFluentDisplay.Show)
        {
            Display = display;
        }
    }
}
