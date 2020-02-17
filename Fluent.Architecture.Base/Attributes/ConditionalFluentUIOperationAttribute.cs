using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Enumerator;
using Newtonsoft.Json;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ConditionalFluentUIOperationAttribute : Attribute
    {
        public string ObservedProperty { get; private set; }

        public string TriggerValue { get; private set; }

        [JsonIgnore]
        public object Value { get => value; set { this.value = value.ToString() ?? null; } }

        private string value { get; set; }

        public EnumFluentUITriggetEvent TriggetEvent { get; set; } = EnumFluentUITriggetEvent.CHANGE;

        public EnumFluentUIOperation Operation { get; set; } = EnumFluentUIOperation.SHOW;

        public EnumFluentUIOperationType OperatorType { get; set; } = EnumFluentUIOperationType.EQUAL;

        [JsonIgnore]
        public override object TypeId => base.TypeId;

        public ConditionalFluentUIOperationAttribute() { }

        public ConditionalFluentUIOperationAttribute(string observedProperty, object triggerValue)
        {
            ObservedProperty = observedProperty;
            TriggerValue = triggerValue?.ToFluentJson();
        }

        public ConditionalFluentUIOperationAttribute(string observedProperty, object triggerValue, EnumFluentUIOperation operation)
        {
            ObservedProperty = observedProperty;
            TriggerValue = triggerValue?.ToFluentJson();
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