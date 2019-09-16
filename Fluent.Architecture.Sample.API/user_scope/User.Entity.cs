using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users"), DbType(FluentDbType.SQLITE)]
public class User : FluentEntity
{
    [Key]
    public long Code { get; set; }

    [FluentUniqueKey]
    public string Email { get; set; }

    [Searchable]
    public string Name { get; set; }

    public string Andress { get; set; }
}