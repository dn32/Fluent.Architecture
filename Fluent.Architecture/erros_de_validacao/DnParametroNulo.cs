using Newtonsoft.Json;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnParametroNuloErroDeValidacao : DnValorNuloErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => nameof(DnParametroNuloErroDeValidacao);

        public DnParametroNuloErroDeValidacao(string parametro)
            : base($"O parâmetro {parametro} não deveria ser nulo.", parametro)
        {
        }
    }
}