using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
using Newtonsoft.Json;
using System;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteAggregationAttribute : FluenteReferenceAttribute
    {
        public string Display { get; set; }
        public string[] PropertiesForFind { get; set; }
        public bool AllowAdd { get; set; }

        [JsonIgnore]
        public string PropertyForFind
        {
            get
            {
                return PropertiesForFind?.Length > 0 ? PropertiesForFind[0] : null;
            }
            set
            {
                PropertiesForFind = new[] { value };
            }
        }

        public FluenteFilterAttribute FluenteFilter { get; set; }
    }


    public class FluenteLoggingAttribute : Attribute
    {
        public EnumMostrar Display { get; set; }

        public FluenteLoggingAttribute(EnumMostrar display = EnumMostrar.Mostrar)
        {
            Display = display;
        }
    }
}
