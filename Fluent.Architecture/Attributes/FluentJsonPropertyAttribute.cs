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
#pragma warning disable IDE1006 // Naming Styles
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
        public int LGrid { get; set; }

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

        public FluentCompositionAttribute? FluentComposition { get; set; }

        public FluentAggregationAttribute? FluentAggregation { get; set; }

        public bool IsEnum { get; set; }
        public bool IsKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsList { get; set; }
        public List<KeyValuePair<string, string>> Enums { get; set; }

        public int Row { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
