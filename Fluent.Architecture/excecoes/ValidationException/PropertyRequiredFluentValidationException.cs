using dn32.infra.Extensoes;
using Newtonsoft.Json;
using System.Reflection;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class CampoDeTelaRequiredDnErroDeValidacao : DnCampoDeTelaErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => "TheFieldMustHaveAValueForThisOperation";

        public CampoDeTelaRequiredDnErroDeValidacao(PropertyInfo propriedade, string propriedadeDeComposicao, string campoDeComposicao) :
            base(propriedade, true, $"The field {(campoDeComposicao == null ? propriedade.GetUiPropertyName() : campoDeComposicao + "." + propriedade.GetUiPropertyName())} must have a valor for this operation.", propriedadeDeComposicao, campoDeComposicao)
        {
        }
    }
}