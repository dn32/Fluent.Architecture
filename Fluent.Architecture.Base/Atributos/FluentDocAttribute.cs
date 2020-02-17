using Fluente.Arquitetura.Nucleo.Enumerator;
using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property)]
    public class FluenteDocAttribute : Attribute
    {
        public EnumFluenteDisplay Display { get; private set; }
        
        public FluenteDocAttribute(EnumFluenteDisplay display = EnumFluenteDisplay.Show)
        {
            Display = display;
        }
    }
}
