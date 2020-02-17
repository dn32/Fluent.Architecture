using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    public abstract class FluenteValidateAttribute : Attribute
    {
        public abstract bool IsValidWhen(object value);
        public abstract string InvalidMessage { get; }
        public abstract object ExampleValue { get; }
        public object Entity { get; internal set; }
    }
}
