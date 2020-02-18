using dn32.infra.Nucleo.Models;
using System;
using dn32.infra.enumeradores;

namespace dn32.infra.nucleo.atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DnComposicaoAtributo : DnReferenciaAtributo
    {
        public DnJsonSchema Formulario { get; set; }
        public EnumTipoDeOperacaoParaComAsReferencias OperacaoAoSalvar { get; set; } = EnumTipoDeOperacaoParaComAsReferencias.AdicionarEAtualizar;
    }
}
