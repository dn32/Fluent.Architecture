using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Enum;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Models
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