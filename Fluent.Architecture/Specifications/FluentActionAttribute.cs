using System;

namespace dn32.infra.Nucleo.Specifications
{
    public class DnActionAttribute : Attribute
    {
        public bool Pagination { get; set; }
        public bool DynamicSpec { get; set; }
    }
}
