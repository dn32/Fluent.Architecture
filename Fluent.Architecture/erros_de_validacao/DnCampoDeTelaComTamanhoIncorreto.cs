using dn32.infra.extensoes;
using Newtonsoft.Json;
using System.Reflection;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnCampoDeTelaComTamanhoIncorretoErroDeValidacao : DnCampoDeTelaErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => nameof(DnCampoDeTelaComTamanhoIncorretoErroDeValidacao);

        public int Minimo { get; set; }

        public double Maximo { get; set; }

        public DnCampoDeTelaComTamanhoIncorretoErroDeValidacao(
            PropertyInfo propriedade, 
            string propriedadeDeComposicao, 
            string campoDeComposicao) :
            base(propriedade, true, $"O campo {(campoDeComposicao == null ? propriedade.GetUiPropertyName() : campoDeComposicao + "." + propriedade.GetUiPropertyName())} possui uma quantidade indevida de caracteres.", propriedadeDeComposicao, campoDeComposicao)
        {
            var ret = propriedade.GetPropertyRange();
            this.Minimo = ret?.min ?? 0;
            this.Maximo = ret?.max ?? 0;

            this.Campo = propriedade.GetUiPropertyName();
        }
    }
}