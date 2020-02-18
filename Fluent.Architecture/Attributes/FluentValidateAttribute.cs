using System;

namespace dn32.infra.Nucleo.Atributos
{
    public abstract class FluenteValidateAttribute : Attribute
    {
        public abstract bool IsValidWhen(object value);
        public abstract string InvalidMessage { get; }
        public abstract object ExampleValue { get; }
        public object Entity { get; internal set; }
    }
}
