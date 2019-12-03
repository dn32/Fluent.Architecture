using Fluent.Architecture.Enumerator;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ConditionalFluentUIOperationAttribute : Attribute
    {
        public string ObservedProperty { get; set; }
        public string TriggerValue { get; set; }
        public string Value { get; set; }
        public EnumFluentUITriggetEvent TriggetEvent { get; set; } = EnumFluentUITriggetEvent.CHANGE;
        public EnumFluentUIOperation Operation { get; set; } = EnumFluentUIOperation.SHOW;
        public EnumFluentUIOperationType OperatorType { get; set; } = EnumFluentUIOperationType.EQUAL;

        public ConditionalFluentUIOperationAttribute() { }

        public ConditionalFluentUIOperationAttribute(string observedProperty, object triggerValue)
        {
            ObservedProperty = observedProperty;
            TriggerValue = triggerValue?.ToString() ?? null;
        }

        public ConditionalFluentUIOperationAttribute(string observedProperty, object triggerValue, EnumFluentUIOperation operation)
        {
            ObservedProperty = observedProperty;
            TriggerValue = triggerValue?.ToString() ?? null;
            Operation = operation;
        }

        public ConditionalFluentUIOperationAttribute(string observedProperty, EnumFluentUITriggetEvent triggetEvent, EnumFluentUIOperation operation)
        {
            ObservedProperty = observedProperty;
            TriggetEvent = triggetEvent;
            Operation = operation;
        }
    }
}