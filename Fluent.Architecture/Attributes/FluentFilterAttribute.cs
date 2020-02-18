using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteFilterAttribute : Attribute
    {
        public EnumTipoDeFiltro FilterType { get; set; } = EnumTipoDeFiltro.Igual;

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
