using dn32.infra.Nucleo.Extensoes;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace dn32.infra.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteReferenceAttribute : Attribute
    {
        public string[] LocalKeys { get; set; }

        public string[] ExternalKeys { get; set; }

        [JsonIgnore]
        public string LocalKey
        {
            get
            {
                return ExternalKeys.First();
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
                return ExternalKeys.First();
            }
            set
            {
                ExternalKeys = new[] { value };
            }
        }

        public void SetType(string type) => Type = type;

        public void SetName(string propertyName) => PropertyName = propertyName.ToFluenteJsonStringNormalized();

        public string Type { get; private set; }

        public string PropertyName { get; private set; }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
