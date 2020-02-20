using Newtonsoft.Json;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnEntidadeExisteErroDeValidacao : DnErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => nameof(DnEntidadeExisteErroDeValidacao);

        public DnEntidadeExisteErroDeValidacao(string chavesDaEntidade) :
            base($"Uma entidade com essas chaves já existe no banco de dados: {chavesDaEntidade}", false, chavesDaEntidade)
        {
        }
    }
}