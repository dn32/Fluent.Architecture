using System;
using System.Collections.Generic;

namespace Fluent.Architecture.Core.TestSupport
{
    public class FluentNode
    {
        public Type EntityType { get; set; }
        public string EntityTypeName => EntityType.Name;
        public List<FluentNode> ReferencePointers { get; set; } = new List<FluentNode>();
        public bool IsPrimitive { get; set; }
        public object Instance { get; set; }
    }
}
