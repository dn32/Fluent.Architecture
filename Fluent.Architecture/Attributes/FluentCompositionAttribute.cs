using Fluente.Arquitetura.Nucleo.Enumerator;
using Fluente.Arquitetura.Nucleo.Models;
using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteCompositionAttribute : FluenteReferenceAttribute
    {
        public FluenteJsonSchema Form { get; set; }
        public EnumOnSaveReference OnSave { get; set; } = EnumOnSaveReference.ADD_AND_UPDATE;
    }
}
