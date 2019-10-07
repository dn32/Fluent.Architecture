using System;

namespace Fluent.Architecture.Core.Attributes
{
    public abstract class FluentValidateAttribute : Attribute
    {
        public abstract bool IsValidWhen(object value);
        public abstract string InvalidMessage { get; }
        public abstract object ExampleValue { get; }
        public object Entity { get; internal set; }
    }
}
