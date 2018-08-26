using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using Fluent.Architecture.Test.SupportElements.Mock;

namespace Fluent.Architecture.Test.SupportElements.Model
{
    [Table("users")]
    public class User : FluentEntity
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public ePersonType? PersonType { get; set; }

        [Required]
        public string Name { get; set; }

        [FluentUnicKey]
        public string UserName { get; set; }

        [FluentUnicKey]
        public string Email { get; set; }

        public string Tel { get; set; }

        public string Password { get; set; }
    }
}