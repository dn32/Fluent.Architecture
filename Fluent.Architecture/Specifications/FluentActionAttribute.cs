using System;
using System.Collections.Generic;
using System.Text;

namespace Fluente.Arquitetura.Nucleo.Specifications
{
    public class FluenteActionAttribute: Attribute
    {
        public bool Pagination { get; set; }
        public bool DynamicSpec { get; set; }
    }
}
