using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FluenteAPIControllerAttribute : Attribute
    {
        public bool AutomaticGeneration { get; set; }

        public FluenteAPIControllerAttribute()
        {
            AutomaticGeneration = true;
        }

        public FluenteAPIControllerAttribute(bool automaticGeneration = true)
        {
            AutomaticGeneration = automaticGeneration;
        }
    }
}
