using Fluente.Arquitetura.Nucleo.Models;
using System;
using dn32.infra.enumeradores;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteCompositionAttribute : FluenteReferenceAttribute
    {
        public FluenteJsonSchema Form { get; set; }
        public EnumTipoDeOperacaoParaComAsReferencias OnSave { get; set; } = EnumTipoDeOperacaoParaComAsReferencias.AdicionarEAtualizar;
    }
}
