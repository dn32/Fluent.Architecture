using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;

[Table("Users"), DbType(FluentDbType.SQLITE)]
public class User : FluentEntity
{
    [Key]
    public int Code { get; set; }

    [FluentUniqueKey]
    public string Email { get; set; }

    [Searchable]
    public string Name { get; set; }
}