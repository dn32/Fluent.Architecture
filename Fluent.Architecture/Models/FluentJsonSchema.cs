using Fluent.Architecture.Core.Attributes;
using System.Collections.Generic;

namespace Fluent.Architecture.Core.Models
{
    public class FluentJsonSchema
    {
        public FluentJsonFormAttribute FluentJsonForm { get; set; }
        public List<FluentJsonPropertyAttribute> Properties { get; set; }
    }
}
