using Newtonsoft.Json;
using System;

namespace dn32.infra.nucleo.atributos
{
    public class DnEsquemaJsonAtributo : Attribute
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string NomeDaPropriedade { get; set; }

        public string Link { get; set; }

        [JsonIgnore]
        public string NomeDaPropriedadeCaseSensitive { get; set; }

        public string Grupo { get; set; }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
