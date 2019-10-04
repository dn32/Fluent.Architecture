using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    public class FluentActionSchema
    {
        public Type EntityType { get; set; }
        public Type ControllerType { get; set; }
        public MethodInfo Action { get; set; }
        public string Method { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public int OrderMethod { get; internal set; }
        public IEnumerable<DocParameter> Parameters { get; set; }
        public string Description { get; internal set; }
    }
}
