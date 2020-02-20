using dn32.infra.Extensoes;
using Newtonsoft.Json;
using System.Reflection;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class CampoDeTelaLenghtDnErroDeValidacao : DnCampoDeTelaErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => "TheFieldLenghtDnValidationException";

        public int Min { get; set; }

        public double Max { get; set; }

        public CampoDeTelaLenghtDnErroDeValidacao(PropertyInfo propriedade, string propriedadeDeComposicao, string campoDeComposicao) :
            base(propriedade, true, $"The {(campoDeComposicao == null ? propriedade.GetUiPropertyName() : campoDeComposicao + "." + propriedade.GetUiPropertyName())} field has more or less characters than allowed.", propriedadeDeComposicao, campoDeComposicao)
        {
            var ret = propriedade.GetPropertyRange();
            Min = ret?.min ?? 0;
            Max = ret?.max ?? 0;

            this.Campo = propriedade.GetUiPropertyName();
        }
    }
}