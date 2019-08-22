// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("MXSDEPTO"), DbType(FluentDbType.ORACLE)]
public class Departamento : FluentEntity
{
    [Key, Column("CODEPTO")]
    public string Codepto { get; set; }

    [Column("DESCRICAO")]
    public string Descricao { get; set; }
}