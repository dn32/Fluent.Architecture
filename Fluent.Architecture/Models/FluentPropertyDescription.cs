using System;

namespace Fluent.Architecture.Core.Models
{
    public class FluentPropertyDescription
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public Type DynamicProperty { get; set; }

        public FluentClassDescription FluentClassDescription { get; set; }
    }
}
