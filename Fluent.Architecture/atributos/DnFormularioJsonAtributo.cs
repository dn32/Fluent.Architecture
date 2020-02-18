using Newtonsoft.Json;
using System;

namespace dn32.infra.nucleo.atributos
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DnFormularioJsonAtributo : DnEsquemaJsonAtributo
    {
        [JsonIgnore]
        public Type Tipo { get; set; }

        public bool EhTabelaIntermediaria { get; set; }

        public bool EhSomenteLeitura { get; set; }
    }
}
