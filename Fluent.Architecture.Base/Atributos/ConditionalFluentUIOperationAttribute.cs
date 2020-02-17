using Fluente.Arquitetura.Nucleo.Extensoes;
using Fluente.Arquitetura.Enumeradores;
using Newtonsoft.Json;
using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ConditionalFluenteUIOperationAttribute : Attribute
    {
        public string ObservedProperty { get; private set; }

        public string TriggerValue { get; private set; }

        [JsonIgnore]
        public object Value { get => value; set { this.value = value.ToString() ?? null; } }

        private string value { get; set; }

        public EnumFluenteUITriggetEvent TriggetEvent { get; set; } = EnumFluenteUITriggetEvent.CHANGE;

        public EnumFluenteUIOperation Operation { get; set; } = EnumFluenteUIOperation.SHOW;

        public EnumFluenteUIOperationType OperatorType { get; set; } = EnumFluenteUIOperationType.EQUAL;

        [JsonIgnore]
        public override object TypeId => base.TypeId;

        public ConditionalFluenteUIOperationAttribute() { }

        public ConditionalFluenteUIOperationAttribute(string observedProperty, object triggerValue)
        {
            ObservedProperty = observedProperty;
            TriggerValue = triggerValue?.ToFluenteJson();
        }

        public ConditionalFluenteUIOperationAttribute(string observedProperty, object triggerValue, EnumFluenteUIOperation operation)
        {
            ObservedProperty = observedProperty;
            TriggerValue = triggerValue?.ToFluenteJson();
            Operation = operation;
        }

        public ConditionalFluenteUIOperationAttribute(string observedProperty, EnumFluenteUITriggetEvent triggetEvent, EnumFluenteUIOperation operation)
        {
            ObservedProperty = observedProperty;
            TriggetEvent = triggetEvent;
            Operation = operation;
        }
    }
}