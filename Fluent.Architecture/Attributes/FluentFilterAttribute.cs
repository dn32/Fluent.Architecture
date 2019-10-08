using Fluent.Architecture.Core.Enumerator;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentFilterAttribute : Attribute
    {
        public EnumFilterType FilterType { get; set; } = EnumFilterType.EQUAL;

        public string[] LocalKeys { get; set; }

        public string[] ExternalKeys { get; set; }

        public string[] FieldsToClear { get; set; }

        [JsonIgnore]
        public string LocalKey
        {
            get
            {
                return ExternalKeys?.First();
            }
            set
            {
                LocalKeys = new[] { value };
            }
        }

        [JsonIgnore]
        public string ExternalKey
        {
            get
            {
                return ExternalKeys?.First();
            }
            set
            {
                ExternalKeys = new[] { value };
            }
        }

        [JsonIgnore]
        public string FieldToClear
        {
            get
            {
                return FieldsToClear?.First();
            }
            set
            {
                FieldsToClear = new[] { value };
            }
        }

        [JsonIgnore]
        public override object TypeId => base.TypeId;

        public string PropertyName { get; internal set; }
    }
}
