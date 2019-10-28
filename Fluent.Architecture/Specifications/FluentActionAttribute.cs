using System;
using System.Collections.Generic;
using System.Text;

namespace Fluent.Architecture.Core.Specifications
{
    public class FluentActionAttribute: Attribute
    {
        public bool Pagination { get; set; }
        public bool DynamicSpec { get; set; }
    }
}
