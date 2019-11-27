using Newtonsoft.Json;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FluentJsonFormAttribute : FluentJsoSchemaAttribute
    {
        [JsonIgnore]
        public Type Type { get; set; }

        public bool IsIntermediateTable { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
