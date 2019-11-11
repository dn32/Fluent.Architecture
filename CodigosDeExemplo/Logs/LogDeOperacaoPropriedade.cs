using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Max.Infraestrutura.config.Logs
{
    [FluentLogging(EnumFluentDisplay.Hidden)]
    [FluentAPIController(AutomaticGeneration = false)]
    [Table("MXS_LOGDEOPERACAOPROPRIEDADES")]
    public class LogDeOperacaoPropriedade : FluentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long LogDeOperacaoEntidadeId { get; set; }
        public string Nome { get; set; }
        public string ValorOriginal { get; set; }
        public string NovoValor { get; set; }
    }
}
