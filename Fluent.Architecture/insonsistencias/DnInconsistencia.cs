using dn32.infra.nucleo.erros_de_validacao;
using Newtonsoft.Json;

namespace dn32.infra.nucleo.insonsistencias
{
    public class DnInconsistencia
    {
        public string Mensagem { get; set; }

        public string ChaveDeGlobalizacao { get; set; }

        [JsonIgnore]
        public DnErroDeValidacao DnErroDeValidacao { get; set; }
    }
}
