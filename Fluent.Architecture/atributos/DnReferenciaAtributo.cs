using dn32.infra.Nucleo.Extensoes;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace dn32.infra.nucleo.atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DnReferenciaAtributo : Attribute
    {
        public string[] ChavesLocais { get; set; }

        public string[] ChavesExternas { get; set; }

        [JsonIgnore]
        public string ChaveLocal
        {
            get => this.ChavesExternas.First();
            set
            {
                this.ChavesLocais = new[] { value };
            }
        }

        [JsonIgnore]
        public string ChaveExterna
        {
            get => this.ChavesExternas.First();
            set
            {
                this.ChavesExternas = new[] { value };
            }
        }

        public void DefinirTipo(string tipo) => this.Tipo = tipo;

        public void DefinirNome(string nomeDaPropriedade) => this.NomeDaPropriedade = nomeDaPropriedade.ToDnJsonStringNormalized();

        public string Tipo { get; private set; }

        public string NomeDaPropriedade { get; private set; }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
