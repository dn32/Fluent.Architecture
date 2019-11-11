using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Max.Infraestrutura.config.Logs
{
    [FluentLogging(EnumFluentDisplay.Hidden)]
    [FluentAPIController(AutomaticGeneration = false)]
    [Table("MXS_LOGDEOPERACOES")]
    public class LogDeOperacao : FluentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(255)]
        public string Usuario { get; set; }

        public DateTime Data { get; set; }

        public EnumOrigemLogDeOperacao Origem { get; set; }

        [MaxLength(500)]
        public string Metodo { get; set; }

        [ForeignKey(nameof(LogDeOperacaoEntidade.LogDeOperacaoId))]
        public List<LogDeOperacaoEntidade> Entidades { get; set; }
    }
}
