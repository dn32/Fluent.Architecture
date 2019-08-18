// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fluent.Architecture.Sample.Test.SupportElements.Model
{
    [Table("users"), DbType(FluentDbType.SQLITE)]
    public class User : FluentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public EnumPersonType? PersonType { get; set; }

        [Required, JsonProperty("full_name"), Searchable]
        public string Name { get; set; }

        [FluentUniqueKey, Searchable]
        public string UserName { get; set; }

        [FluentUniqueKey, Searchable]
        public string Email { get; set; }

        public string Tel { get; set; }

        public long ZipCode { get; set; }

        public string Password { get; set; }
    }
}