using dn32.infra.extensoes;
using Newtonsoft.Json;
using System.Reflection;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnCampoDeTelaRequeridoErroDeValidacao : DnCampoDeTelaErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => nameof(DnCampoDeTelaRequeridoErroDeValidacao);

        public DnCampoDeTelaRequeridoErroDeValidacao(PropertyInfo propriedade, string propriedadeDeComposicao, string campoDeComposicao) :
            base(propriedade, true, $"O campo '{(campoDeComposicao == null ? propriedade.GetUiPropertyName() : campoDeComposicao + "." + propriedade.GetUiPropertyName())} deve possuir um valor.", propriedadeDeComposicao, campoDeComposicao)
        {
        }
    }
}