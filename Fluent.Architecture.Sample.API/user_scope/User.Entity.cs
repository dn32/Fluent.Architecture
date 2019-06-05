using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;

[Table("Users"), DbType(FluentDbType.MYSQL)]
public class User : FluentEntity
{
    [Key]
    public int Code { get; set; }

    [FluentUniqueKey]
    public string Email { get; set; }

    public string Name { get; set; }
}