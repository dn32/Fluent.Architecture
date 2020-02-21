using Newtonsoft.Json;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnValorNuloErroDeValidacao : DnErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => nameof(DnValorNuloErroDeValidacao);

        public DnValorNuloErroDeValidacao(string mensagem, string parametro = null) : base(mensagem, false, parametro)
        {
        }
    }
}