
using dn32.infra.nucleo.erros_de_validacao;
using Newtonsoft.Json;

namespace dn32.infra.Nucleo.Inconsistences
{
    public class DnInconsistence
    {
        public string Message { get; set; }
        public string ChaveDeGlobalizacao { get; set; }
        [JsonIgnore]
        public DnErroDeValidacao DnException { get; set; }
    }

    public class DnPropertyInconsistence : DnInconsistence
    {
        public string PropertyName { get; set; }
    }

    public class DnUiFieldInconsistence : DnPropertyInconsistence
    {
        public string Field { get; set; }
    }
}
