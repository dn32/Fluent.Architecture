using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fluent.Architecture.Sample
{
    [DbType(FluentDbType.ORACLE)]
    [Table("ERP_MXSBAIRRO")]
    public class Bairro : FluentEntity
    {
        [Key]
        public string CODBAIRRO { get; set; }
        public string DESCRICAO { get; set; }
        public string UF { get; set; }
        public string CODCIDADE { get; set; }
        public long VLTXENTREGA { get; set; }
        public long FATORMULTIPLICADOR { get; set; }
        public long ATUALIZID { get; set; }
        public DateTime DTATUALIZ { get; set; }
        public int CODOPERACAO { get; set; }
    }
}
