using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Fluent.Architecture.Core.Attributes
{
    public class FluentJsoSchemaAttribute : Attribute
    {
        [DisplayName("Name")]
        [JsonProperty("Name")]
        public string name { get; set; }

        [DisplayName("Description")]
        [JsonProperty("Description")]
        public string desc { get; set; }

        [DisplayName("PropertyName")]
        [JsonProperty("PropertyName")]
        public string propName { get; set; }

        [DisplayName("Group")]
        [JsonProperty("Group")]
        public string group { get; set; }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
