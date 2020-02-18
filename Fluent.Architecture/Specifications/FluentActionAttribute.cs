using System;

namespace dn32.infra.Nucleo.Specifications
{
    public class FluenteActionAttribute : Attribute
    {
        public bool Pagination { get; set; }
        public bool DynamicSpec { get; set; }
    }
}
