using Newtonsoft.Json;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnPropriedadeASerFiltradaNaoEncontradaErroDeValidacao : DnErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => nameof(DnPropriedadeASerFiltradaNaoEncontradaErroDeValidacao);

        public DnPropriedadeASerFiltradaNaoEncontradaErroDeValidacao(
            string nomeDaEntidade,
            string nomeDaPropriedade)
            : base($"A entidade '{nomeDaEntidade}' não possui uma propriedade" +
                   $" com nome '{nomeDaPropriedade}' para aplicar o filtro.")
        {
        }
    }
}