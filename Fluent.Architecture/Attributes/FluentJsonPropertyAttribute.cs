using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using dn32.infra.atributos;
using dn32.infra.enumeradores;

namespace dn32.infra.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteJsonPropertyAttribute : FluenteJsoSchemaAttribute
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
        public EnumTipoDeComponenteDeFormularioDeTela form { get; set; }

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
        public EnumTipoDeAgrupamentoDeTela tgroup { get; set; }

        [DisplayName("DefaultValue")]
        [JsonProperty("DefaultValue")]
        public object value { get; set; }

        public FluenteCompositionAttribute FluenteComposition { get; set; }

        public FluenteAggregationAttribute FluenteAggregation { get; set; }

        public bool IsEnum { get; set; }
        public bool IsKey { get; set; }
        public bool IsFluenteUniqueKeyKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsList { get; set; }
        public List<KeyValuePair<string, string>> Enums { get; set; }

        public int Row { get; set; }

        public Type Type { get; set; }

        [JsonIgnore]
        public PropertyInfo Property { get; set; }

        [JsonIgnore]
        public FluenteJsonFormAttribute FkDestinal { get; internal set; }

        [JsonIgnore]
        public bool IsFk { get; internal set; }

        public IEnumerable<FluenteOperacaoDeCondicionalDeTelaAtributo> ConditionalFluenteUIOperations { get; internal set; }
    }
}
