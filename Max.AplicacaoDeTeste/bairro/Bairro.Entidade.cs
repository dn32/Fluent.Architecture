using Fluent.Architecture.EntityFramework;
using Max.Infraestrutura.ClassesBase;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Max.AplicacaoDeTeste.bairro
{
    //[DbType(FluentDbType.ORACLE)]
    [DbType(FluentDbType.MYSQL)]
    [Table("ERP_MXSBAIRRO")]
    public class Bairro : MaxEntidade
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
