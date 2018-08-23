using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.SampleFrameworkWeb.Models
{
    [Table("users")]
    public class User : FluentEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [FluentUnicKey]
        public string Email { get; set; }
    }
}