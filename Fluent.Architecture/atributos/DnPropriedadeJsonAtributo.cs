using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using dn32.infra.atributos;
using dn32.infra.enumeradores;

namespace dn32.infra.nucleo.atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DnPropriedadeJsonAtributo : DnEsquemaJsonAtributo
    {
        public bool EhRequerido { get; set; }

        public int Minimo { get; set; }

        public double Maximo { get; set; }

        public EnumTipoDeComponenteDeFormularioDeTela Formulario { get; set; }

        public int LayoutDeGrid { get; set; }

        public string Grid { get; set; }

        public EnumTipoDeAgrupamentoDeTela TipoDeAgrupamento { get; set; }

        public object Valor { get; set; }

        public DnComposicaoAtributo Composicao { get; set; }

        public DnAgregacaoAtributo Agregacao { get; set; }

        public bool EhEnumerador { get; set; }

        public bool EhChave { get; set; }

        public bool EhDnChaveUnica { get; set; }

        public bool PermiteNulo { get; set; }

        public bool EhLista { get; set; }

        public List<KeyValuePair<string, string>> Enumeradores { get; set; }

        public int Linha { get; set; }

        public Type Tipo { get; set; }

        [JsonIgnore]
        public PropertyInfo Propriedade { get; set; }

        [JsonIgnore]
        public DnFormularioJsonAtributo DestinoDeChaveExterna { get; internal set; }

        [JsonIgnore]
        public bool EhChaveExterna { get; internal set; }

        public IEnumerable<DnOperacaoDeCondicionalDeTelaAtributo> OperacaoDeCondicional { get; internal set; }
    }
}
