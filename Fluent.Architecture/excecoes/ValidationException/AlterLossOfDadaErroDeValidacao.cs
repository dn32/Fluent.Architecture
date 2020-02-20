using Newtonsoft.Json;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class AlterLossOfDadaErroDeValidacao : DnErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => nameof(AlterLossOfDadaErroDeValidacao);

        public AlterLossOfDadaErroDeValidacao()
            : base($"This operation physically removes all data from the requested table. If you really want to do this, you should add to the request header the term \"APAGAR_TUDO=YES\"")
        {
        }
    }
}