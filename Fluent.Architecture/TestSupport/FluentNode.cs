using System;
using System.Collections.Generic;

namespace Fluente.Arquitetura.Nucleo.TestSupport
{
    public class FluenteNode
    {
        public Type EntityType { get; set; }
        public string EntityTypeName => EntityType.Name;
        public List<FluenteNode> ReferencePointers { get; set; } = new List<FluenteNode>();
        public bool IsPrimitive { get; set; }
        public object Instance { get; set; }
    }
}
