using System;

namespace dn32.infra.Nucleo.Models
{
    internal class DnPropertyDescription
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public Type DynamicProperty { get; set; }

        public DnClassDescription DnClassDescription { get; set; }
    }
}
