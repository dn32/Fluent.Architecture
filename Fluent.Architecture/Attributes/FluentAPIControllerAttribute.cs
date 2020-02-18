using System;

namespace dn32.infra.Nucleo.Atributos
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
