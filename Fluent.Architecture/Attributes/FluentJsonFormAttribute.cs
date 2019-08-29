using Fluent.Architecture.Core.Enumerator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Fluent.Architecture.Core.Attributes
{
    public class FluentJsonFormAttribute : Attribute
    {
        [DisplayName("Required")]
        [JsonProperty("Required")]
        public bool required;

        [DisplayName("Name")]
        [JsonProperty("Name")]
        public string name { get; set; }

        [DisplayName("Description")]
        [JsonProperty("Description")]
        public string desc { get; set; }

        [DisplayName("Minimum")]
        [JsonProperty("Minimum")]
        public int min { get; set; }

        [DisplayName("Maximum")]
        [JsonProperty("Maximum")]
        public int max { get; set; }

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

        [DisplayName("PropertyName")]
        [JsonProperty("PropertyName")]
        public string propName { get; set; }

        public FluentCompositionAttribute FluentComposition { get; set; }

        public FluentAggregationAttribute FluentAggregation { get; set; }

        public bool IsEnum { get; set; }

        public List<KeyValuePair<string, string>> Enums { get; set; }

        public int Row { get; set; }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
