using Newtonsoft.Json;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnFalhaNaRemocaoDeDadosErroDeValidacao : DnErroDeValidacao
    {
        [JsonProperty("chave_de_globalizacao")]
        public override string ChaveDeGlobalizacao => nameof(DnFalhaNaRemocaoDeDadosErroDeValidacao);

        public DnFalhaNaRemocaoDeDadosErroDeValidacao()
            : base($"Esta operação remove fisicamente todos os dados da tabela solicitada. " +
                   $"Se você realmente deseja fazer isso, adicione ao cabeçalho da solicitação o " +
                   $"termo \"APAGAR_TUDO=yes\" ")
        {
        }
    }
}