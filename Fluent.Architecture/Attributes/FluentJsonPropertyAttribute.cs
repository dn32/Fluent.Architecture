using Fluent.Architecture.Core.Enumerator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentJsonPropertyAttribute : FluentJsoSchemaAttribute
    {
        [DisplayName("Required")]
        [JsonProperty("Required")]
        public bool required { get; set; }

        [DisplayName("Minimum")]
        [JsonProperty("Minimum")]
        public int min { get; set; }

        [DisplayName("Maximum")]
        [JsonProperty("Maximum")]
        public double max { get; set; }

        [DisplayName("Form")]
        [JsonProperty("Form")]
        public EnumForm form { get; set; }

        [DisplayName("LayoutGrid")]
        [JsonProperty("LayoutGrid")]
        public int lGrid { get; set; }

        [DisplayName("GridTitle")]
        [JsonProperty("GridTitle")]
        public string grid { get; set; }

        [DisplayName("Reference")]
        [JsonProperty("Reference")]
        public string reference { get; set; }

        [DisplayName("GroupType")]
        [JsonProperty("GroupType")]
        public EnumGrupType tgroup { get; set; }

        [DisplayName("DefaultValue")]
        [JsonProperty("DefaultValue")]
        public object value { get; set; }

        public FluentCompositionAttribute FluentComposition { get; set; }

        public FluentAggregationAttribute FluentAggregation { get; set; }

        public bool IsEnum { get; set; }
        public bool IsKey { get; set; }
        public bool IsFluentUniqueKeyKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsList { get; set; }
        public List<KeyValuePair<string, string>> Enums { get; set; }

        public int Row { get; set; }

        public Type Type { get; set; }

        [JsonIgnore]
        public FluentJsonFormAttribute FkDestinal { get; internal set; }

        [JsonIgnore]
        public bool IsFk { get; internal set; }

        public IEnumerable<ConditionalFluentUIOperationAttribute> ConditionalFluentUIOperations { get; internal set; }
    }
}
