using dn32.infra.nucleo.insonsistencias;
using System.Collections.Generic;

namespace dn32.infra.nucleo.modelos
{
    public class DnRetornoDeValidacao
    {
        public string Mensagem { get; set; }

        public bool EhErroDeValidacao { get; set; }

        public List<DnInconsistencia> Inconsistencias { get; set; }
    }
}