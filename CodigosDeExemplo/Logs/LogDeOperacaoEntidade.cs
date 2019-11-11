using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Max.Infraestrutura.config.Logs
{
    [FluentLogging(EnumFluentDisplay.Hidden)]
    [FluentAPIController(AutomaticGeneration = false)]
    [Table("MXS_LOGDEOPERACAOENTIDADES")]
    public class LogDeOperacaoEntidade : FluentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long LogDeOperacaoId { get; set; }

        [MaxLength(500)]
        public string Chaves { get; set; }

        [MaxLength(400)]
        public string Entidade { get; set; }

        public EnumLogDeOperacaoTipoDeOperacao TipoDeOperacao { get; set; }
       
        [ForeignKey(nameof(LogDeOperacaoPropriedade.LogDeOperacaoEntidadeId))]
        public List<LogDeOperacaoPropriedade> Propriedades { get; set; }
    }
}
