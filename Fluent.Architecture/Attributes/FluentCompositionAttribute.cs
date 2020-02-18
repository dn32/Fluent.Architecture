using Fluente.Arquitetura.Nucleo.Models;
using System;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteCompositionAttribute : FluenteReferenceAttribute
    {
        public FluenteJsonSchema Form { get; set; }
        public EnumTipoDeOperacaoParaComAsReferencias OnSave { get; set; } = EnumTipoDeOperacaoParaComAsReferencias.AdicionarEAtualizar;
    }
}
