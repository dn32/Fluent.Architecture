using Fluent.Architecture.Core.Enumerator;
using Newtonsoft.Json;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentFilterAttribute : Attribute
    {
        public EnumFilterType FilterType { get; set; } = EnumFilterType.EQUAL;
        public string ExternalProperty { get; set; }
        public string LocalProperty { get; set; }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
