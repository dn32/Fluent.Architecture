using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    public class FluenteJsoSchemaAttribute : Attribute
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

        public string Link { get; set; }

        [JsonIgnore]
        public string PropNameCaseSensitive { get; set; }

        [DisplayName("Group")]
        [JsonProperty("Group")]
        public string group { get; set; }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
