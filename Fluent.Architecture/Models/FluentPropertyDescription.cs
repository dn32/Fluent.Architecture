using System;

namespace Fluente.Arquitetura.Nucleo.Models
{
    internal class FluentePropertyDescription
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public Type DynamicProperty { get; set; }

        public FluenteClassDescription FluenteClassDescription { get; set; }
    }
}
