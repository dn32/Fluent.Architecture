using Fluente.Arquitetura.Nucleo.Enumerator;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteFilterAttribute : Attribute
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
